using Microsoft.AspNetCore.Mvc;
using Superhero.Entities;
using Superhero.Services;

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
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            var heroes = await _superHeroService.GetAllSuperHeroesAsync();
            return Ok(heroes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroWithId(int id)
        {
            var hero = await _superHeroService.GetSuperHeroByIdAsync(id);
            if (hero == null) return NotFound();
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateNewSuperHero(SuperHero hero)
        {
            await _superHeroService.CreateSuperHeroAsync(hero);
            var heroes = await _superHeroService.GetAllSuperHeroesAsync();
            return Ok(heroes);
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero updatedHero)
        {
            await _superHeroService.UpdateSuperHeroAsync(updatedHero);
            return Ok(await _superHeroService.GetAllSuperHeroesAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int heroId)
        {
            await _superHeroService.DeleteSuperHeroAsync(heroId);
            var heroes = await _superHeroService.GetAllSuperHeroesAsync();
            return Ok(heroes);
        }
    }
}
