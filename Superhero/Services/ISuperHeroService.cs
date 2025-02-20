using Superhero.DTOs;

namespace Superhero.Services
{
    public interface ISuperHeroService
    {
        Task<List<SuperHeroDetailsDto>> GetAllSuperHeroesAsync();
        Task<SuperHeroDetailsDto?> GetSuperHeroByIdAsync(int id);
        Task CreateSuperHeroAsync(SuperHeroDto newHeroDetails);
        Task<SuperHeroDetailsDto?> UpdateSuperHeroAsync(int id, SuperHeroDto updatedHeroDetails);
        Task DeleteSuperHeroAsync(int id);
    }
}
