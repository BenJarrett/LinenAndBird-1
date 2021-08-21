using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.Models
{
    public class Bird
    {
        public Guid Id { get; set; }

        public BirdType Type { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }

        public List<string> Accessories { get; set; }
    }

    public enum BirdType
    {
        Dead,
        Linen
    }
}
