using Microsoft.EntityFrameworkCore;

namespace Mutify.Models
{
    public class MutifyContext : DbContext
    {
        public MutifyContext(DbContextOptions<MutifyContext> opts) : base(opts)
        {
        }

        public DbSet<Track> Tracks { get; set; }
    }
}