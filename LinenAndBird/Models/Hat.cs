using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.Models // Models are a representation of your data in C#.
{
    public class Hat
    {
        public string Designer { get; set; }
        public string Color { get; set; }
        public HatStyle Style { get; set; }
    }

    public enum HatStyle
    {
        Normal,
        OpenBack,
        WideBrim
    }
}

// Functionally, they are just classes with nothing but properties in them.
// You'll also hear people call these classes with only properties "POCOs" for Plain Old C# Object
