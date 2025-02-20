using Superhero.Common;

namespace Superhero.DTOs
{
    public class SuperHeroDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Place { get; set; }

        public SuperHeroDetailsDto(int id, string name, string firstName, string lastName, string place)
        {
            Id = id;
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Place = place;
        }
    }
}
