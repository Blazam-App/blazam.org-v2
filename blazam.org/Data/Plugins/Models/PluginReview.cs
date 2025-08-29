using System.ComponentModel.DataAnnotations;

namespace blazam.org.Data.Plugins.Models
{
    public class PluginReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string? ReviewText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PluginId { get; set; }
        public virtual BlazamPlugin Plugin { get; set; }

        public int UserId { get; set; }
        public virtual PluginUser User { get; set; }
    }
}