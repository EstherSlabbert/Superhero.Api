using Superhero.Common;
using Superhero.Entities;

namespace UnitTests.Entities
{
    public class SuperHeroEntityTests()
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void When_Creating_SuperHero_With_Invalid_Name_Throws_InvalidParameterException(string name)
        {
            //Arrange & Act
            var act = () => new SuperHero(name);

            //Assert
            act.ShouldThrow<InvalidParameterException>(@$"The argument ""name"" was not provided.");
        }

        [Fact]
        public void When_Creating_SuperHero_With_Valid_Properties()
        {
            //Arrange & Act
            var newSuperHero = new SuperHero("Wonder Woman", "Diana", "Price", "Washington, D.C.");

            //Assert
            newSuperHero.Id.ShouldBeOfType<int>();
            newSuperHero.Name.ShouldBe(newSuperHero.Name);
            newSuperHero.FirstName.ShouldBe(newSuperHero.FirstName);
            newSuperHero.LastName.ShouldBe(newSuperHero.LastName);
            newSuperHero.Place.ShouldBe(newSuperHero.Place);
        }

        [Fact]
        public void When_Updating_SuperHero_With_UpdateHero_Updates_Successfully()
        {
            //Arrange
            var existingHero = new SuperHero("Iron Man", "Tony", "Stark", "Malibu");
            var newName = "Iron Man 2.0";
            var newFirstName = "Anthony";
            var newLastName = "Stark Jr.";
            var newPlace = "New York City";

            //Act
            existingHero.UpdateHero(newName, newFirstName, newLastName, newPlace);

            //Assert
            existingHero.Name.ShouldBe(newName);
            existingHero.FirstName.ShouldBe(newFirstName);
            existingHero.LastName.ShouldBe(newLastName);
            existingHero.Place.ShouldBe(newPlace);
        }
    }
}