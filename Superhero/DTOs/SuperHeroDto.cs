using Superhero.Common;

namespace Superhero.DTOs
{
    public class SuperHeroDto
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Place { get; set; }

        public SuperHeroDto(string name, string firstName = "", string lastName = "", string place = "")
        {
            name.ThrowIfNullOrWhiteSpace();

            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Place = place;
        }
    }
}
