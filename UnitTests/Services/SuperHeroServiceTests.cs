using Moq;
using Superhero.Common;
using Superhero.DTOs;
using Superhero.Entities;
using Superhero.Repositories;
using Superhero.Services;

namespace UnitTests.Services;

public class SuperHeroServiceTests
{
    private readonly Mock<ISuperHeroRepository> _mockRepository;
    private readonly SuperHeroService _service;

    public SuperHeroServiceTests()
    {
        _mockRepository = new Mock<ISuperHeroRepository>();
        _service = new SuperHeroService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllSuperHeroesAsync_ShouldReturnListOfHeroesWithTheCorrectDetails()
    {
        // Arrange
        var heroes = new List<SuperHero>
        {
            new() {
                Id = 1,
                Name = "Superman",
                FirstName = "Clark",
                LastName = "Kent",
                Place = "Metropolis"
            },
            new() {
                Id = 2,
                Name = "Batman",
                FirstName = "Bruce",
                LastName = "Wayne",
                Place = "Gotham"
            }
        };

        _mockRepository.Setup(r => r.GetAllSuperHeroesAsync())
            .ReturnsAsync(heroes);

        // Act
        var result = await _service.GetAllSuperHeroesAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        result[0].Id.ShouldBe(heroes[0].Id);
        result[0].Name.ShouldBe(heroes[0].Name);
        result[0].FirstName.ShouldBe(heroes[0].FirstName);
        result[0].LastName.ShouldBe(heroes[0].LastName);
        result[0].Place.ShouldBe(heroes[0].Place);

        result[1].Id.ShouldBe(heroes[1].Id);
        result[1].Name.ShouldBe(heroes[1].Name);
        result[1].FirstName.ShouldBe(heroes[1].FirstName);
        result[1].LastName.ShouldBe(heroes[1].LastName);
        result[1].Place.ShouldBe(heroes[1].Place);
    }

    [Fact]
    public async Task GetSuperHeroByIdAsync_WithValidId_ShouldReturnHeroWithCorrectDetails()
    {
        // Arrange
        var hero = new SuperHero
        {
            Id = 1,
            Name = "Supergirl",
            FirstName = "Kara",
            LastName = "Zor-El",
            Place = "National City"
        };

        _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(hero.Id))
            .ReturnsAsync(hero);

        // Act
        var result = await _service.GetSuperHeroByIdAsync(hero.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(hero.Id);
        result.Name.ShouldBe(hero.Name);
        result.FirstName.ShouldBe(hero.FirstName);
        result.LastName.ShouldBe(hero.LastName);
        result.Place.ShouldBe(hero.Place);
    }

    [Fact]
    public async Task GetSuperHeroByIdAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((SuperHero?)null);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
            await _service.GetSuperHeroByIdAsync(999));
    }

    [Fact]
    public async Task CreateSuperHeroAsync_ShouldCallRepository()
    {
        // Arrange
        var newHero = new SuperHero
        {
            Id = 1,
            Name = "Flash",
            FirstName = "Barry",
            LastName = "Allen",
            Place = "Central City"
        };
        var newHeroDetails = new SuperHeroDto(newHero.Name, newHero.FirstName, newHero.LastName, newHero.Place);

        _mockRepository.Setup(r => r.CreateSuperHeroAsync(newHeroDetails))
            .Returns(Task.FromResult(newHero));

        // Act
        await _service.CreateSuperHeroAsync(newHeroDetails);

        // Assert
        _mockRepository.Verify(r => r.CreateSuperHeroAsync(newHeroDetails), Times.Once);
    }

    [Fact]
    public async Task UpdateSuperHeroAsync_WithValidId_ShouldReturnUpdatedHeroWithCorrectDetails()
    {
        // Arrange
        var existingHero = new SuperHero
        {
            Id = 7,
            Name = "Wonder Woman",
            FirstName = "Diana",
            LastName = "Price",
            Place = "Washington, D.C."
        };
        var updatedHero = new SuperHeroDto(existingHero.Name, existingHero.FirstName, existingHero.LastName, existingHero.Place);

        _mockRepository.Setup(r => r.UpdateSuperHeroAsync(existingHero.Id, updatedHero))
            .ReturnsAsync(existingHero);

        // Act
        var result = await _service.UpdateSuperHeroAsync(existingHero.Id, updatedHero);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(existingHero.Id);
        result.Name.ShouldBe(existingHero.Name);
        result.FirstName.ShouldBe(existingHero.FirstName);
        result.LastName.ShouldBe(existingHero.LastName);
        result.Place.ShouldBe(existingHero.Place);
    }

    [Fact]
    public async Task UpdateSuperHeroAsync_ShouldUpdateAllFields()
    {
        // Arrange
        var existingHero = new SuperHero
        {
            Id = 2,
            Name = "Iron Man",
            FirstName = "Tony",
            LastName = "Stark",
            Place = "Malibu"
        };
        var updatedHero = new SuperHeroDto("Iron Man 2.0", "Anthony", "Stark Jr.", "New York");

        _mockRepository.Setup(r => r.UpdateSuperHeroAsync(existingHero.Id, updatedHero))
            .ReturnsAsync(new SuperHero(
                updatedHero.Name,
                updatedHero.FirstName,
                updatedHero.LastName,
                updatedHero.Place
            )
            {
                Id = existingHero.Id
            });

        // Act
        var result = await _service.UpdateSuperHeroAsync(existingHero.Id, updatedHero);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(existingHero.Id);
        result.Name.ShouldBe(updatedHero.Name);
        result.FirstName.ShouldBe(updatedHero.FirstName);
        result.LastName.ShouldBe(updatedHero.LastName);
        result.Place.ShouldBe(updatedHero.Place);
    }

    [Fact]
    public async Task UpdateSuperHeroAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        var updatedHero = new SuperHeroDto("Batman", "Bruce", "Wayne", "Gotham");

        _mockRepository.Setup(r => r.UpdateSuperHeroAsync(It.IsAny<int>(), updatedHero))
            .ReturnsAsync((SuperHero?)null);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
            await _service.UpdateSuperHeroAsync(999, updatedHero));
    }

    [Fact]
    public async Task DeleteSuperHeroAsync_WithValidId_ShouldCallRepository()
    {
        // Arrange
        var existingHero = new SuperHero
        {
            Id = 10,
            Name = "Spiderman",
            FirstName = "Peter",
            LastName = "Parker",
            Place = "NYC"
        };

        _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(existingHero.Id))
            .ReturnsAsync(existingHero);

        _mockRepository.Setup(r => r.DeleteSuperHeroAsync(existingHero.Id))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteSuperHeroAsync(existingHero.Id);

        // Assert
        _mockRepository.Verify(r => r.DeleteSuperHeroAsync(existingHero.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteSuperHeroAsync_WithInvalidId_ShouldThrowException()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((SuperHero?)null);

        // Act & Assert
        await Should.ThrowAsync<EntityNotFoundException>(async () =>
            await _service.DeleteSuperHeroAsync(999));
    }
}
