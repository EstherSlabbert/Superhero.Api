using Superhero.Entities;

namespace Superhero.Services
{
    public interface ISuperHeroService
    {
        Task<List<SuperHero>> GetAllSuperHeroesAsync();
        Task<SuperHero?> GetSuperHeroByIdAsync(int id);
        Task<SuperHero> CreateSuperHeroAsync(SuperHero hero);
        Task<SuperHero> UpdateSuperHeroAsync(SuperHero hero);
        Task DeleteSuperHeroAsync(int id);
    }
}
