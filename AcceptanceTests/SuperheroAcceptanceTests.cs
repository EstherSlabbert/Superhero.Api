using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Superhero.Data;
using Superhero.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace AcceptanceTests
{
    public class SuperHeroApiTests : IClassFixture<SuperheroWebApplicationFactory>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly DataContext _dbContext;
        private int? _testHeroId;

        public SuperHeroApiTests(SuperheroWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            // Resolve the database context for cleanup
            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        }

        // Runs before each test to ensure a clean database
        public async Task InitializeAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM SuperHeroes");
            
            // Add default test superhero
            var testHero = new Superhero.Entities.SuperHero
            {
                Name = "Test Hero",
                FirstName = "Test",
                LastName = "Hero",
                Place = "Test City"
            };
            _dbContext.SuperHeroes.Add(testHero);
            await _dbContext.SaveChangesAsync();
            
            // Store the generated ID for test assertions
            _testHeroId = testHero.Id;
        }


        // Runs after each test (optional, for additional cleanup)
        public async Task DisposeAsync()
        {
            await _dbContext.Database.ExecuteSqlRawAsync("DELETE FROM SuperHeroes");
        }

        [Fact]
        public async Task GetAllSuperHeroes_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync("/api/SuperHero");

            // Assert
            response.EnsureSuccessStatusCode();
            var heroes = await response.Content.ReadFromJsonAsync<List<SuperHeroDetailsDto>>();
            Assert.NotNull(heroes);
            Assert.NotEmpty(heroes);
        }

        [Fact]
        public async Task CreateSuperHero_ReturnsSuccess()
        {
            // Arrange
            var newHero = new SuperHeroDto("New Test Hero", "New Test", "New Hero", "New Test City");

            // Act
            var response = await _client.PostAsJsonAsync("/api/SuperHero", newHero);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateSuperHero_StoresSuperHeroInDb()
        {
            // Arrange
            var newHero = new SuperHeroDto("New Test Hero", "New", "Test", "New City");

            // Act
            var response = await _client.PostAsJsonAsync("/api/SuperHero", newHero);
            response.EnsureSuccessStatusCode();
            
            // Assert
            var createdHero = await _dbContext.SuperHeroes
                .FirstOrDefaultAsync(h => h.Name == "New Test Hero");
                
            Assert.NotNull(createdHero);
            Assert.Equal(newHero.Name, createdHero.Name);
            Assert.Equal(newHero.FirstName, createdHero.FirstName);
            Assert.Equal(newHero.LastName, createdHero.LastName);
            Assert.Equal(newHero.Place, createdHero.Place);
        }

        [Fact]
        public async Task UpdateSuperHero_ReturnsSuccess()
        {
            // Arrange
            var updatedHero = new SuperHeroDto("Updated Hero", "Updated", "Hero", "Updated City");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/SuperHero/{_testHeroId}", updatedHero);

            // Assert
            response.EnsureSuccessStatusCode();
            var hero = await response.Content.ReadFromJsonAsync<SuperHeroDetailsDto>();
            Assert.NotNull(hero);
            Assert.Equal(updatedHero.Name, hero.Name);
        }
        public async Task UpdateSuperHero_UpdatesDataInDatabase()
        {
            // Arrange
            var updatedHero = new SuperHeroDto("Updated Hero", "Updated", "Hero", "Updated City");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/SuperHero/{_testHeroId}", updatedHero);

            // Assert
            response.EnsureSuccessStatusCode();

            var dbHero = await _dbContext.SuperHeroes.FindAsync(_testHeroId);
            Assert.NotNull(dbHero);
            Assert.Equal(updatedHero.Name, dbHero.Name);
            Assert.Equal(updatedHero.FirstName, dbHero.FirstName);
            Assert.Equal(updatedHero.LastName, dbHero.LastName);
            Assert.Equal(updatedHero.Place, dbHero.Place);
        }

        [Fact]
        public async Task DeleteSuperHero_ReturnsNoContent()
        {
            // Act
            var response = await _client.DeleteAsync($"/api/SuperHero/{_testHeroId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetSuperHeroById_ReturnsSuccess()
        {
            // Act
            var response = await _client.GetAsync($"/api/SuperHero/{_testHeroId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var hero = await response.Content.ReadFromJsonAsync<SuperHeroDetailsDto>();
            Assert.NotNull(hero);
            Assert.Equal(_testHeroId, hero.Id);
        }

        [Fact]
        public async Task GetSuperHeroById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var response = await _client.GetAsync($"/api/SuperHero/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            // Verify the hero doesn't exist in database
            var dbHero = await _dbContext.SuperHeroes.FindAsync(invalidId);
            Assert.Null(dbHero);
        }
    }
}
