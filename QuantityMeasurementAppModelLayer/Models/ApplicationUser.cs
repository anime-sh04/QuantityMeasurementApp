using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementAppModelLayer.Models
{
    [Table("Users")]
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        /// <summary>
        /// SHA-256 hashed password (null for Google-only accounts).
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Google subject identifier from the ID token (null for local accounts).
        /// </summary>
        [MaxLength(200)]
        public string? GoogleId { get; set; }

        [MaxLength(20)]
        public string Role { get; set; } = "User";   // "User" | "Admin"

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        // ── Navigation: one user owns many measurement records ────────────────
        public ICollection<QuantityMeasurementEntity> Measurements { get; set; } = [];
    }
}
