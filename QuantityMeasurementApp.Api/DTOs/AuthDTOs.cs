using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Api.DTOs
{
    // ── Registration / Login ──────────────────────────────────────────────────

    public class RegisterRequestDTO
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }
    }

    public class LoginRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class GoogleLoginRequestDTO
    {
        /// <summary>The ID token returned by Google Sign-In on the frontend.</summary>
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }

    // ── Responses ─────────────────────────────────────────────────────────────

    public class AuthResponseDTO
    {
        public string AccessToken  { get; set; } = string.Empty;
        public string TokenType    { get; set; } = "Bearer";
        public int    ExpiresIn    { get; set; }   // seconds
        public UserProfileDTO User { get; set; } = new();
    }

    public class UserProfileDTO
    {
        public int    Id        { get; set; }
        public string Email     { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName  { get; set; }
        public string Role      { get; set; } = string.Empty;
    }
}
