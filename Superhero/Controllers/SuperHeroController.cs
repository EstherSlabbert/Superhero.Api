using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Superhero.Data;
using Superhero.Entities;

namespace Superhero.Controllers
{
    // all logic is here, but usually there are services injected
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuperHeroController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeroes()
        {
            var heroes = await _dataContext.SuperHeroes.ToListAsync();
            return Ok(heroes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SuperHero>> GetSuperHeroWithId(int id)
        {
            var hero = await _dataContext.SuperHeroes.FindAsync(id);
            if (hero == null) return NotFound();
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateNewSuperHero(SuperHero hero)
        {
            _dataContext.SuperHeroes.Add(hero);
            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateSuperHero(SuperHero updatedHero)
        {
            var dbHero = await _dataContext.SuperHeroes.FindAsync(updatedHero.Id);
            if (dbHero == null) return NotFound();

            dbHero.Name = updatedHero.Name;
            dbHero.FirstName = updatedHero.FirstName;
            dbHero.LastName = updatedHero.LastName;
            dbHero.Place = updatedHero.Place;

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }

        [HttpDelete]
        public async Task<ActionResult<List<SuperHero>>> DeleteSuperHero(int heroId)
        {
            var heroToDelete = await _dataContext.SuperHeroes.FindAsync(heroId);
            if (heroToDelete == null) return NotFound();

            _dataContext.SuperHeroes.Remove(heroToDelete);

            await _dataContext.SaveChangesAsync();
            return Ok(await _dataContext.SuperHeroes.ToListAsync());
        }
    }
}
