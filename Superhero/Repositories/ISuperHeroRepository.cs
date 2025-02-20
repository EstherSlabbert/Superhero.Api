using Superhero.DTOs;
using Superhero.Entities;

namespace Superhero.Repositories
{
    public interface ISuperHeroRepository
    {
        Task<List<SuperHero>> GetAllSuperHeroesAsync();
        Task<SuperHero?> GetSuperHeroByIdAsync(int id);
        Task<SuperHero> CreateSuperHeroAsync(SuperHeroDto hero);
        Task<SuperHero?> UpdateSuperHeroAsync(int id, SuperHeroDto updatedHeroDetails);
        Task DeleteSuperHeroAsync(int id);
    }
}
