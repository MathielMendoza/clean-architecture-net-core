using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Data
{
    public class StreamerDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=gerbo-qa.database.windows.net,1433;
                                          Initial Catalog=Streamer;
                                          Persist Security Info=False;
                                          User ID=user-develop;
                                          Password=S1st3m@s*;
                                          MultipleActiveResultSets=False;
                                          Encrypt=True;
                                          TrustServerCertificate=False;
                                          Connection Timeout=30;")
               .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
                   Microsoft.Extensions.Logging.LogLevel.Information)
               .EnableSensitiveDataLogging();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streamer>()
                .HasMany(m => m.Videos)
                .WithOne(m => m.Streamer)
                .HasForeignKey(m => m.StreamerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Video>()
                .HasMany(p => p.Actores)
                .WithMany(t => t.Videos)
                .UsingEntity<VideoActor>(
                    pt => pt.HasKey(e => new { e.ActorId, e.VideoId })
                );

        
        }


        public DbSet<Streamer>? Streamers { get; set; }

        public DbSet<Video>? Videos { get; set; }

        public DbSet<Actor>? Actores { get; set; }

        public DbSet<Director>? Directores { get; set; }

    }
}
