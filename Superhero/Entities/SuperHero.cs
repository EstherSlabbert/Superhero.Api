using Superhero.Common;

namespace Superhero.Entities
{
    public class SuperHero : Entity
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Place { get; set; }
        public SuperHero() { }
        public SuperHero(string name, string firstName = "", string lastName = "", string place = "")
        {
            name.ThrowIfNullOrWhiteSpace();

            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Place = place;
        }

        public void UpdateHero(string name, string firstName = "", string lastName = "", string place = "")
        {
            name.ThrowIfNullOrWhiteSpace();

            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Place = place;
        }
    }
}
