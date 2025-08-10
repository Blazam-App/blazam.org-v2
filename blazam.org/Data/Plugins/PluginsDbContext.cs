using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace blazam.org.Data.Plugins
{
    public class PluginsDbContext : DbContext
    {
        public static string? ConnectionString { get; set; }

        public PluginsDbContext() : base() { }

        public PluginsDbContext(DbContextOptions<PluginsDbContext> options) : base(options) { }

        public DbSet<PluginUser> Users { get; set; }
        public DbSet<PendingPluginUser> PendingUsers { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<PluginVerification> Verifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && ConnectionString != null)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Plugin
            modelBuilder.Entity<Plugin>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Plugins)
                .HasForeignKey(p => p.AuthorId);

            // Configure one-to-one relationship between User and Verification
            modelBuilder.Entity<PluginVerification>()
                .HasOne(v => v.PendingUser)
                .WithOne(u => u.Verification)
                .HasForeignKey<PluginVerification>(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

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

        // Navigation properties
        public virtual ICollection<Plugin> Plugins { get; set; } = new List<Plugin>();
        public virtual PluginVerification Verification { get; set; }
    }

    public class Plugin
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int Downloads { get; set; } = 0;

        public int AuthorId { get; set; }
        public virtual PluginUser Author { get; set; }
    }

    public class PendingPluginUser : PluginUser
    {

    }

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