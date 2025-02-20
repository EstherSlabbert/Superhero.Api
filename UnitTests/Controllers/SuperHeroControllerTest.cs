using Microsoft.AspNetCore.Mvc;
using Moq;
using Superhero.Controllers;
using Superhero.DTOs;
using Superhero.Entities;
using Superhero.Repositories;
using Superhero.Services;

namespace UnitTests.Controllers
{
    public class SuperHeroControllerTest
    {
        private readonly Mock<ISuperHeroRepository> _mockRepository;
        private readonly SuperHeroService _service;
        private readonly SuperHeroController _controller;
        public SuperHeroControllerTest()
        {
            _mockRepository = new Mock<ISuperHeroRepository>();
            _service = new SuperHeroService(_mockRepository.Object);
            _controller = new SuperHeroController(_service);
        }

        [Fact]
        public async Task Sends_SuperHeroDto_Correctly_When_Calling_CreateNewSuperHero_Then_Returns_Ok()
        {
            //Arrange
            var requestDto = new SuperHeroDto("Supergirl", "Kara", "Zor-El", "National City");
            //Act
            var response = await _controller.CreateNewSuperHero(requestDto);
            //Assert
            response.ShouldBeOfType<OkResult>();
        }

        [Fact]
        public async Task Sends_Id_When_Calling_DeleteSuperHero_Then_Returns_Ok()
        {
            //Arrange
            var existingHero = new SuperHero
            {
                Id = 1,
                Name = "SuperGirl",
                FirstName = "Cara",
                LastName = "Zor-El",
                Place = "Nationale City"
            };

            _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(existingHero.Id))
            .ReturnsAsync(existingHero);

            _mockRepository.Setup(r => r.DeleteSuperHeroAsync(existingHero.Id))
            .Returns(Task.CompletedTask);

            //Act
            var response = await _controller.DeleteSuperHero(existingHero.Id);
            //Assert
            response.ShouldBeOfType<OkResult>();
        }

        [Fact]
        public async Task Sends_SuperHeroDto_Correctly_When_Calling_UpdateSuperHero_Then_Returns_OkObjectResult()
        {
            //Arrange
            var existingHero = new SuperHero
            {
                Id = 1,
                Name = "SuperGirl",
                FirstName = "Cara",
                LastName = "Zor-El",
                Place = "Nationale City"
            };

            var requestDto = new SuperHeroDto("Supergirl", "Kara", "Zor-El", "National City");

            _mockRepository.Setup(r => r.UpdateSuperHeroAsync(existingHero.Id, requestDto))
            .ReturnsAsync(new SuperHero(
                requestDto.Name,
                requestDto.FirstName,
                requestDto.LastName,
                requestDto.Place
            )
            {
                Id = existingHero.Id
            });

            //Act
            var response = await _controller.UpdateSuperHero(existingHero.Id, requestDto);
            //Assert
            response.ShouldBeOfType<ActionResult<SuperHeroDetailsDto>>();
            response.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Sends_Id_Correctly_When_Calling_GetSuperHeroWithId_Then_Returns_OkObjectResult()
        {
            //Arrange
            var existingHero = new SuperHero
            {
                Id = 1,
                Name = "SuperGirl",
                FirstName = "Cara",
                LastName = "Zor-El",
                Place = "Nationale City"
            };

            _mockRepository.Setup(r => r.GetSuperHeroByIdAsync(existingHero.Id))
            .ReturnsAsync(existingHero);

            //Act
            var response = await _controller.GetSuperHeroWithId(existingHero.Id);
            //Assert
            response.ShouldBeOfType<ActionResult<SuperHeroDetailsDto>>();
            response.Result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Sends_Request_Correctly_When_Calling_GetAllSuperHeroes_Then_Returns_OkObjectResult()
        {
            //Arrange
            var existingHeroes = new List<SuperHero>
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
            .ReturnsAsync(existingHeroes);

            //Act
            var response = await _controller.GetAllSuperHeroes();
            //Assert
            response.ShouldBeOfType<ActionResult<List<SuperHeroDetailsDto>>>();
            response.Result.ShouldBeOfType<OkObjectResult>();
        }
    }
}
