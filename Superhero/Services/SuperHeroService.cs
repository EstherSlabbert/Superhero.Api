using Superhero.Common;
using Superhero.DTOs;
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

        public async Task<List<SuperHeroDetailsDto>> GetAllSuperHeroesAsync()
        {
            var heroes = await _repository.GetAllSuperHeroesAsync();
            var heroesDtos = heroes.Select(GetSuperHeroDetailsDto).ToList();
            return heroesDtos;
        }

        public async Task<SuperHeroDetailsDto?> GetSuperHeroByIdAsync(int id)
        {
            var hero = await _repository.GetSuperHeroByIdAsync(id) ?? throw new EntityNotFoundException($"Hero with id \"{id}\" was not found.");
            var heroDto = GetSuperHeroDetailsDto(hero);
            return heroDto;
        }

        public async Task<SuperHeroDetailsDto> CreateSuperHeroAsync(SuperHeroDto newHeroDetails)
        {
            var newHero = await _repository.CreateSuperHeroAsync(newHeroDetails);
            var newHeroDto = GetSuperHeroDetailsDto(newHero);
            return newHeroDto;
        }

        public async Task<SuperHeroDetailsDto?> UpdateSuperHeroAsync(int id, SuperHeroDto updatedHeroDetails)
        {
            var hero = await _repository.UpdateSuperHeroAsync(id, updatedHeroDetails) ?? throw new EntityNotFoundException($"Hero with id \"{id}\" was not found.");
            var heroDto = GetSuperHeroDetailsDto(hero);
            return heroDto;
        }

        public async Task DeleteSuperHeroAsync(int id)
        {
            var hero = await _repository.GetSuperHeroByIdAsync(id) ?? throw new EntityNotFoundException($"Hero with id \"{id}\" was not found.");
            await _repository.DeleteSuperHeroAsync(id);
        }

        public SuperHeroDetailsDto GetSuperHeroDetailsDto(SuperHero hero)
        {
            return new SuperHeroDetailsDto
            (
                hero.Id,
                hero.Name,
                hero.FirstName,
                hero.LastName,
                hero.Place
            );
        }
    }
}
