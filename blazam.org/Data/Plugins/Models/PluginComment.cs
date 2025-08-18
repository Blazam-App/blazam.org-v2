using System.ComponentModel.DataAnnotations;

namespace blazam.org.Data.Plugins.Models
{
    public class PluginComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int PluginId { get; set; }
        public virtual BlazamPlugin Plugin { get; set; }

        public int UserId { get; set; }
        public virtual PluginUser User { get; set; }
    }
}