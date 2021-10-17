using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.Models
{
    public class Order
    {
        public Bird Bird { get; set; }
        public Hat Hat { get; set; }

        public double Price { get; set; }
    }
}
