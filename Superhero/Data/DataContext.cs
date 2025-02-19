using Microsoft.EntityFrameworkCore;
using Superhero.Entities;

namespace Superhero.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<SuperHero> SuperHeroes { get; set; } //Table name
    }
}
