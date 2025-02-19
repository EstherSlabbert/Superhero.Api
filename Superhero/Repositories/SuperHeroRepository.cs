using Microsoft.EntityFrameworkCore;
using Superhero.Data;
using Superhero.Entities;

namespace Superhero.Repositories
{
    public class SuperHeroRepository : ISuperHeroRepository
    {
        private readonly DataContext _context;

        public SuperHeroRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<SuperHero>> GetAllSuperHeroesAsync()
        {
            return await _context.SuperHeroes.ToListAsync();
        }

        public async Task<SuperHero?> GetSuperHeroByIdAsync(int id)
        {
            return await _context.SuperHeroes.FindAsync(id);
        }

        public async Task<SuperHero> CreateSuperHeroAsync(SuperHero hero)
        {
            _context.SuperHeroes.Add(hero);
            await _context.SaveChangesAsync();
            return hero;
        }

        public async Task<SuperHero> UpdateSuperHeroAsync(SuperHero hero)
        {
            _context.SuperHeroes.Update(hero);
            await _context.SaveChangesAsync();
            return hero;
        }

        public async Task DeleteSuperHeroAsync(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero != null)
            {
                _context.SuperHeroes.Remove(hero);
                await _context.SaveChangesAsync();
            }
        }
    }
}
