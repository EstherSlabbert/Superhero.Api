using Superhero.Entities;

namespace Superhero.Repositories
{
    public interface ISuperHeroRepository
    {
        Task<List<SuperHero>> GetAllSuperHeroesAsync();
        Task<SuperHero?> GetSuperHeroByIdAsync(int id);
        Task<SuperHero> CreateSuperHeroAsync(SuperHero hero);
        Task<SuperHero> UpdateSuperHeroAsync(SuperHero hero);
        Task DeleteSuperHeroAsync(int id);
    }
}
