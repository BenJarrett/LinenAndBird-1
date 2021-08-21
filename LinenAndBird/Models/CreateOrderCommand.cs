using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.Models
{
    public class CreateOrderCommand
    {
        public Guid BirdId { get; set; }
        public Guid HatId { get; set; }
        public double Price { get; set; }
    }
}
