using ApplicationNews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ApplicationNews
{
    public class NewsDbContext : DbContext
    {
        public static string? ConnectionString { get; set; }
        public NewsDbContext():base()
        {
        }

        public NewsDbContext(DbContextOptions options) : base(options)
        {
          
           
        }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Settings> Settings{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
                //if (ConnectionString == null) throw new ApplicationException("ConnectionString is empty. Unable to connect to database!");
                optionsBuilder.UseSqlServer(ConnectionString, options =>
                                  {
                                      options.EnableRetryOnFailure();
                                      //options.SetSqlModeOnOpen();

                                  }).EnableSensitiveDataLogging();
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsItem>(entity =>
            {
            });
        }
    }
}
