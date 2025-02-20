using Microsoft.AspNetCore.Mvc;
using Superhero.Services;
using Superhero.DTOs;

namespace Superhero.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroService _superHeroService;

        public SuperHeroController(ISuperHeroService superHeroService)
        {
            _superHeroService = superHeroService;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHeroDetailsDto>>> GetAllSuperHeroes()
        {
            var heroDtos = await _superHeroService.GetAllSuperHeroesAsync();

            return Ok(heroDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SuperHeroDetailsDto>> GetSuperHeroWithId(int id)
        {
            var heroDto = await _superHeroService.GetSuperHeroByIdAsync(id);

            return Ok(heroDto);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHeroDetailsDto>>> CreateNewSuperHero(SuperHeroDto heroDto)
        {
            await _superHeroService.CreateSuperHeroAsync(heroDto);

            var heroDtos = await _superHeroService.GetAllSuperHeroesAsync();

            return Ok(heroDtos);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<List<SuperHeroDetailsDto>>> UpdateSuperHero([FromRoute] int id, [FromBody] SuperHeroDto updatedHeroDto)
        {
            await _superHeroService.UpdateSuperHeroAsync(id, updatedHeroDto);
            var heroesDtos = await _superHeroService.GetAllSuperHeroesAsync();

            return Ok(heroesDtos);
        }

        [HttpDelete]
        public async Task<ActionResult<List<SuperHeroDetailsDto>>> DeleteSuperHero(int heroId)
        {
            await _superHeroService.DeleteSuperHeroAsync(heroId);
            var heroes = await _superHeroService.GetAllSuperHeroesAsync();
            return Ok(heroes);
        }
    }
}
