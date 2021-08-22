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

// Command is behavioral design pattern that converts requests or simple operations into objects.
// The conversion allows deferred or remote execution of commands, storing command history, etc.
// declares an interface for executing an operation