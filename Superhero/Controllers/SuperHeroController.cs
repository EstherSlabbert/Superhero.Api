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
        public async Task<ActionResult> CreateNewSuperHero(SuperHeroDto heroDto)
        {
            await _superHeroService.CreateSuperHeroAsync(heroDto);

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SuperHeroDetailsDto>> UpdateSuperHero([FromRoute] int id, [FromBody] SuperHeroDto updatedHeroDto)
        {
            var updatedHeroDetailsDto = await _superHeroService.UpdateSuperHeroAsync(id, updatedHeroDto);

            return Ok(updatedHeroDetailsDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSuperHero([FromRoute] int id)
        {
            await _superHeroService.DeleteSuperHeroAsync(id);
            return Ok();
        }
    }
}
