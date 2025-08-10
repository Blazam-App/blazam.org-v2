using blazam.org.Data.Plugins.Models;
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
        public DbSet<BlazamPlugin> Plugins { get; set; }
        public DbSet<PluginVerification> Verifications { get; set; }
        public DbSet<PluginReview> PluginReviews { get; set; }
        public DbSet<PluginComment> PluginComments { get; set; }

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
            modelBuilder.Entity<BlazamPlugin>()
                .HasOne(p => p.Uploader)
                .WithMany(u => u.Plugins)
                .HasForeignKey(p => p.UploaderId);

            // Configure one-to-one relationship between User and Verification
            modelBuilder.Entity<PluginVerification>()
                .HasOne(v => v.PendingUser)
                .WithOne(u => u.Verification)
                .HasForeignKey<PluginVerification>(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure PluginReview relationships
            modelBuilder.Entity<PluginReview>()
                .HasOne(r => r.Plugin)
                .WithMany()
                .HasForeignKey(r => r.PluginId);

            modelBuilder.Entity<PluginReview>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure PluginComment relationships
            modelBuilder.Entity<PluginComment>()
                .HasOne(c => c.Plugin)
                .WithMany()
                .HasForeignKey(c => c.PluginId);

            modelBuilder.Entity<PluginComment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
    }
}