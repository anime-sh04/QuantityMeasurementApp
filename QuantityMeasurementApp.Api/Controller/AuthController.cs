using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Api.Auth;
using QuantityMeasurementApp.Api.DTOs;
using QuantityMeasurementApp.Api.Services;
using QuantityMeasurementAppModelLayer.Models;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementApp.Api.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository      _userRepo;
        private readonly IJwtTokenService     _jwtService;
        private readonly IGoogleTokenValidator _googleValidator;
        private readonly IEncryptionService   _encryption;
        private readonly IConfiguration       _config;

        public AuthController(
            IUserRepository       userRepo,
            IJwtTokenService      jwtService,
            IGoogleTokenValidator googleValidator,
            IEncryptionService    encryption,
            IConfiguration        config)
        {
            _userRepo        = userRepo;
            _jwtService      = jwtService;
            _googleValidator = googleValidator;
            _encryption      = encryption;
            _config          = config;
        }

        // ── POST /api/auth/register ───────────────────────────────────────────

        /// <summary>Register a new local account with email + password.</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth/register
        ///     {
        ///       "email": "alice@example.com",
        ///       "password": "MySecret123!",
        ///       "firstName": "Alice",
        ///       "lastName": "Smith"
        ///     }
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                return Conflict(new { message = "An account with this email already exists." });

            var user = new ApplicationUser
            {
                Email        = dto.Email,
                FirstName    = dto.FirstName,
                LastName     = dto.LastName,
                PasswordHash = _encryption.HashPassword(dto.Password),
                Role         = "User"
            };

            await _userRepo.CreateAsync(user);
            var token = _jwtService.GenerateToken(user);

            return CreatedAtAction(nameof(Me), null, BuildAuthResponse(user, token));
        }

        // ── POST /api/auth/login ──────────────────────────────────────────────

        /// <summary>Login with email + password and receive a JWT.</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth/login
        ///     {
        ///       "email": "alice@example.com",
        ///       "password": "MySecret123!"
        ///     }
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user is null || user.PasswordHash is null ||
                !_encryption.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            if (!user.IsActive)
                return Forbid();

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);
            return Ok(BuildAuthResponse(user, token));
        }

        // ── POST /api/auth/google ─────────────────────────────────────────────

        /// <summary>
        /// Authenticate with a Google ID token obtained from the frontend via Google Sign-In.
        /// Creates a new account automatically on first login.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/auth/google
        ///     {
        ///       "idToken": "eyJhbGciOiJS..."
        ///     }
        /// </remarks>
        [HttpPost("google")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO dto)
        {
            var payload = await _googleValidator.ValidateAsync(dto.IdToken);
            if (payload is null)
                return Unauthorized(new { message = "Invalid or expired Google token." });

            // Try to find existing account by Google subject ID first, then by email.
            var user = await _userRepo.GetByGoogleIdAsync(payload.Subject)
                    ?? await _userRepo.GetByEmailAsync(payload.Email);

            if (user is null)
            {
                // First-time Google login → create account.
                user = new ApplicationUser
                {
                    Email     = payload.Email,
                    FirstName = payload.GivenName,
                    LastName  = payload.FamilyName,
                    GoogleId  = payload.Subject,
                    Role      = "User"
                };
                await _userRepo.CreateAsync(user);
            }
            else
            {
                // Link Google ID if not already linked (user previously registered locally).
                if (user.GoogleId is null)
                {
                    user.GoogleId = payload.Subject;
                }
                user.LastLoginAt = DateTime.UtcNow;
                await _userRepo.UpdateAsync(user);
            }

            if (!user.IsActive)
                return Forbid();

            var token = _jwtService.GenerateToken(user);
            return Ok(BuildAuthResponse(user, token));
        }

        // ── GET /api/auth/me ──────────────────────────────────────────────────

        /// <summary>Return the profile of the currently authenticated user.</summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                       ?? User.FindFirst("sub");

            if (idClaim is null || !int.TryParse(idClaim.Value, out var userId))
                return Unauthorized();

            var user = await _userRepo.GetByIdAsync(userId);
            if (user is null) return NotFound();

            return Ok(new UserProfileDTO
            {
                Id        = user.Id,
                Email     = user.Email,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Role      = user.Role
            });
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private AuthResponseDTO BuildAuthResponse(ApplicationUser user, string token)
        {
            int expiresMinutes = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;
            return new AuthResponseDTO
            {
                AccessToken = token,
                ExpiresIn   = expiresMinutes * 60,
                User = new UserProfileDTO
                {
                    Id        = user.Id,
                    Email     = user.Email,
                    FirstName = user.FirstName,
                    LastName  = user.LastName,
                    Role      = user.Role
                }
            };
        }
    }
}
