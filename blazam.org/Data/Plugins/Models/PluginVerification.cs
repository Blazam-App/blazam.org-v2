using System.ComponentModel.DataAnnotations;

namespace blazam.org.Data.Plugins.Models
{
    public class PluginVerification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int UserId { get; set; }
        public virtual PendingPluginUser PendingUser { get; set; }
    }
}