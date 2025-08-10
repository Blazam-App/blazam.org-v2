using System.ComponentModel.DataAnnotations;

namespace blazam.org.Data.Plugins.Models
{
    public class BlazamPlugin
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public int Downloads { get; set; } = 0;

        public int UploaderId { get; set; }
        public virtual PluginUser Uploader { get; set; }
    }
}