using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Auth;
using AuthService.DTOs;
using AuthService.Repository;
using AuthService.Services;
using Shared.Models;

namespace AuthService.Controllers
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

        /// <summary>Register a new local account.</summary>
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

        /// <summary>Login with email + password.</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            if (user is null || user.PasswordHash is null ||
                !_encryption.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            if (!user.IsActive) return Forbid();

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepo.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);
            return Ok(BuildAuthResponse(user, token));
        }

        /// <summary>Login with a Google ID token.</summary>
        [HttpPost("google")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO dto)
        {
            var payload = await _googleValidator.ValidateAsync(dto.IdToken);
            if (payload is null)
                return Unauthorized(new { message = "Invalid or expired Google token." });

            var user = await _userRepo.GetByGoogleIdAsync(payload.Subject)
                    ?? await _userRepo.GetByEmailAsync(payload.Email);

            if (user is null)
            {
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
                if (user.GoogleId is null) user.GoogleId = payload.Subject;
                user.LastLoginAt = DateTime.UtcNow;
                await _userRepo.UpdateAsync(user);
            }

            if (!user.IsActive) return Forbid();

            var token = _jwtService.GenerateToken(user);
            return Ok(BuildAuthResponse(user, token));
        }

        /// <summary>Return profile of authenticated user.</summary>
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
