using Microsoft.EntityFrameworkCore;
using Superhero.Entities;

namespace Superhero.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // The DbSet<Entity> is the way EntityFramework defines new tables and their contents
        public DbSet<SuperHero> SuperHeroes { get; set; }
        public DbSet<UnhandledException> UnhandledExceptions { get; set; }
    }
}
