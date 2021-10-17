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
                Name = "Jimmy",
                Color = "Red",
                Size = "Small",
                Type = BirdType.Dead,
                Accessories = new List<string> { "Beanie", "Gold Wing Tips" }
            }
        };

        internal IEnumerable<Bird> GetAll()
        {
            return _birds;
        }


        internal void Add(Bird newBird)
        {
            _birds.Add(newBird);
        }
    }
}
