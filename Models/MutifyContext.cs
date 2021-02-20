using Microsoft.EntityFrameworkCore;

namespace Mutify.Models
{
    public class MutifyContext : DbContext
    {
        public MutifyContext(DbContextOptions<MutifyContext> opts) : base(opts)
        {
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BlobFile> BlobFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasIndex(g => new {g.Name}).IsUnique();
        }
    }
}