using Microsoft.EntityFrameworkCore;
using Superhero.Data;
using Superhero.DTOs;
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
            var heroes = await _context.SuperHeroes.ToListAsync();
            return heroes;
        }

        public async Task<SuperHero?> GetSuperHeroByIdAsync(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            return hero;
        }

        public async Task<SuperHero> CreateSuperHeroAsync(SuperHeroDto newHeroDetails)
        {
            var newHero = new SuperHero(newHeroDetails.Name, newHeroDetails.FirstName, newHeroDetails.LastName, newHeroDetails.Place);

            _context.SuperHeroes.Add(newHero);
            await _context.SaveChangesAsync();

            return newHero;
        }

        public async Task<SuperHero?> UpdateSuperHeroAsync(int id, SuperHeroDto updatedHeroDetails)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero is null) return null;

            hero.UpdateHero(updatedHeroDetails.Name, updatedHeroDetails.FirstName, updatedHeroDetails.LastName, updatedHeroDetails.Place);

            _context.SuperHeroes.Update(hero);
            await _context.SaveChangesAsync();

            return hero;
        }

        public async Task DeleteSuperHeroAsync(int id)
        {
            var hero = await _context.SuperHeroes.FindAsync(id);
            if (hero is not null)
            {
                _context.SuperHeroes.Remove(hero);
                await _context.SaveChangesAsync();
            }
        }
    }
}
