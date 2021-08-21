using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.DataAccess
{
    public class BirdRepository
    {
        static List<Bird> _birds = new List<Bird>
        {
            new Bird
            {
                Id = Guid.NewGuid(),
                Name = "Jimmy",
                Color = "Red",
                Size = "Small",
                Type = BirdType.Dead,
                Accessories = new List<string> { "Beanie", "Gold wing tips" }
            }
        };

        internal IEnumerable<Bird> GetAll()
        {
            return _birds;
        }

        internal void Add(Bird newBird)
        {
            newBird.Id = Guid.NewGuid();

            _birds.Add(newBird);
        }

        internal Bird GetById(Guid birdId)
        {
            return _birds.FirstOrDefault(bird => bird.Id == birdId);
        }
    }
}
