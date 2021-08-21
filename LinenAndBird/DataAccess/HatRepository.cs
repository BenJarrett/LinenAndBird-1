using LinenAndBird.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.DataAccess
{
    public class HatRepository
    {
        static List<Hat> _hats = new List<Hat>
            {
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Blue",
                    Designer = "Charlie",
                    Style = HatStyle.OpenBack
                },
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Black",
                    Designer = "Nathan",
                    Style = HatStyle.WideBrim
                },
                new Hat
                {
                    Id = Guid.NewGuid(),
                    Color = "Magenta",
                    Designer = "Charlie",
                    Style = HatStyle.Normal
                }
            };

        internal Hat GetById(Guid hatId)
        {
            return _hats.FirstOrDefault(hat => hat.Id == hatId);
        }

        internal List<Hat> GetAll()
        {
            return _hats;
        }

        internal IEnumerable<Hat> GetByStyle(HatStyle style)
        {
            return _hats.Where(hat => hat.Style == style);
        }

        internal void Add(Hat newHat)
        {
            newHat.Id = Guid.NewGuid();

            _hats.Add(newHat);
        }
    }
}
