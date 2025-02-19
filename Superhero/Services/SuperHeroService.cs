using Superhero.Entities;
using Superhero.Repositories;

namespace Superhero.Services
{
    public class SuperHeroService : ISuperHeroService
    {
        private readonly ISuperHeroRepository _repository;

        public SuperHeroService(ISuperHeroRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SuperHero>> GetAllSuperHeroesAsync()
        {
            return await _repository.GetAllSuperHeroesAsync();
        }

        public async Task<SuperHero?> GetSuperHeroByIdAsync(int id)
        {
            return await _repository.GetSuperHeroByIdAsync(id);
        }

        public async Task<SuperHero> CreateSuperHeroAsync(SuperHero hero)
        {
            return await _repository.CreateSuperHeroAsync(hero);
        }

        public async Task<SuperHero> UpdateSuperHeroAsync(SuperHero hero)
        {
            return await _repository.UpdateSuperHeroAsync(hero);
        }

        public async Task DeleteSuperHeroAsync(int id)
        {
            await _repository.DeleteSuperHeroAsync(id);
        }
    }
}
