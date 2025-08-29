using System.ComponentModel.DataAnnotations;

namespace blazam.org.Data.Plugins.Models
{
    public class PluginUser
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<BlazamPlugin> Plugins { get; set; } = new List<BlazamPlugin>();
        public virtual PluginVerification Verification { get; set; }
    }
}