using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Superhero.Common;
using Superhero.Data;
using Superhero.DTOs;
using Superhero.Entities;
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
        }

        private async Task AddDefaultHeroToDatabase()
        {
            // Add default test superhero
            var testHero = new SuperHero
            {
                Name = "Test Hero",
                FirstName = "Test Firstname",
                LastName = "Test Lastname",
                Place = "Test City"
            };
            await _dbContext.SuperHeroes.AddAsync(testHero);
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
            // Arrange
            await AddDefaultHeroToDatabase();

            // Act
            var response = await _client.GetAsync("/api/SuperHero");

            // Assert
            response.EnsureSuccessStatusCode();
            var heroes = await response.Content.ReadFromJsonAsync<List<SuperHeroDetailsDto>>();
            heroes.ShouldNotBeNull();
            heroes.ShouldNotBeEmpty();
            heroes.ShouldBeOfType<List<SuperHeroDetailsDto>>();
            heroes.ShouldHaveSingleItem();
        }

        [Fact]
        public async Task CreateSuperHero_ReturnsSuccess()
        {
            // Arrange
            var newHero = new SuperHeroDto("New Test Hero", "New Test Name", "New Test Surname", "New Test City");

            // Act
            var response = await _client.PostAsJsonAsync("/api/SuperHero", newHero);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateSuperHero_ReturnsErrorWhenInputIsInvalid()
        {
            // Arrange & Act
            var act = () => _client.PostAsJsonAsync("/api/SuperHero", new SuperHeroDto(null));

            // Assert
            act.ShouldThrow<InvalidParameterException>($@"The argument ""name"" was not provided.");
        }

        [Fact]
        public async Task CreateSuperHero_StoresSuperHeroInDatabase()
        {
            // Arrange
            var newHero = new SuperHeroDto("Wonder Woman", "Diana", "Price", "Washington, D.C.");

            // Act
            var response = await _client.PostAsJsonAsync("/api/SuperHero", newHero);
            response.EnsureSuccessStatusCode();

            // Assert
            var createdHero = await _dbContext.SuperHeroes.AsNoTracking()
                .FirstOrDefaultAsync(h => h.Name == "Wonder Woman");

            createdHero.ShouldNotBeNull();
            createdHero.Name.ShouldBe(newHero.Name);
            createdHero.FirstName.ShouldBe(newHero.FirstName);
            createdHero.LastName.ShouldBe(newHero.LastName);
            createdHero.Place.ShouldBe(newHero.Place);
        }

        [Fact]
        public async Task UpdateSuperHero_ReturnsSuccess()
        {
            // Arrange
            await AddDefaultHeroToDatabase();
            var updatedHero = new SuperHeroDto("Superman", "Clark", "Kent", "Metropolis");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/SuperHero/{_testHeroId}", updatedHero);

            // Assert
            response.EnsureSuccessStatusCode();
            var hero = await response.Content.ReadFromJsonAsync<SuperHeroDetailsDto>();

            hero.ShouldNotBeNull();
            hero.Name.ShouldBe(updatedHero.Name);
            hero.FirstName.ShouldBe(updatedHero.FirstName);
            hero.LastName.ShouldBe(updatedHero.LastName);
            hero.Place.ShouldBe(updatedHero.Place);
        }

        [Fact]
        public async Task UpdateSuperHero_UpdatesSuperHeroInDatabase()
        {
            // Arrange
            await AddDefaultHeroToDatabase();
            var updatedHero = new SuperHeroDto("Batman", "Bruce", "Wayne", "Gotham City");

            // Act
            var response = await _client.PutAsJsonAsync($"/api/SuperHero/{_testHeroId}", updatedHero);

            // Assert
            response.EnsureSuccessStatusCode();

            var dbHero = await _dbContext.SuperHeroes.AsNoTracking()
                .FirstOrDefaultAsync(hero => hero.Id == _testHeroId);
            dbHero.ShouldNotBeNull();
            dbHero.Name.ShouldBe(updatedHero.Name);
            dbHero.FirstName.ShouldBe(updatedHero.FirstName);
            dbHero.LastName.ShouldBe(updatedHero.LastName);
            dbHero.Place.ShouldBe(updatedHero.Place);
        }

        [Fact]
        public async Task DeleteSuperHero_ReturnsSuccess()
        {
            // Arrange
            await AddDefaultHeroToDatabase();

            // Act
            var response = await _client.DeleteAsync($"/api/SuperHero/{_testHeroId}");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteSuperHero_RemovesHeroInDatabase()
        {
            // Arrange
            await AddDefaultHeroToDatabase();

            // Act
            await _client.DeleteAsync($"/api/SuperHero/{_testHeroId}");

            // Assert
            var heroInDb = await _dbContext.SuperHeroes.AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == _testHeroId);
            heroInDb?.ShouldBeNull();
        }

        [Fact]
        public async Task GetSuperHeroById_ReturnsSuccess()
        {
            // Arrange
            await AddDefaultHeroToDatabase();

            // Act
            var response = await _client.GetAsync($"/api/SuperHero/{_testHeroId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var hero = await response.Content.ReadFromJsonAsync<SuperHeroDetailsDto>();

            hero.ShouldNotBeNull();
            hero.Id.ShouldBeEquivalentTo(_testHeroId);
        }

        [Fact]
        public async Task GetSuperHeroById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var response = await _client.GetAsync($"/api/SuperHero/{invalidId}");

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            // Verify the hero doesn't exist in database
            var dbHero = await _dbContext.SuperHeroes.AsNoTracking()
                .FirstOrDefaultAsync(hero => hero.Id == invalidId);
            dbHero?.ShouldBeNull();
        }
    }
}
