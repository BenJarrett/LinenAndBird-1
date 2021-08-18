using LinenAndBird.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinenAndBird.Controllers
{
    [Route("api/hats")] //exposed at this endpoint
    [ApiController] // an api controller, so it returns json or xml; This attribute indicates that the controller responds to web API requests. 
    public class HatsController : ControllerBase // A base class for an MVC controller without view support.
    {
        static List<Hat> _hats = new List<Hat>
            {
                new Hat
                {
                    Color = "Blue",
                    Designer = "Charlie",
                    Style = HatStyle.OpenBack
                },
                new Hat
                {
                    Color = "Black",
                    Designer = "Nathan",
                    Style = HatStyle.WideBrim
                },
                new Hat
                {
                    Color = "Magenta",
                    Designer = "Charlie",
                    Style = HatStyle.Normal
                }
            };

        [HttpGet] // attributes can be used to configure the behavior of web API controllers and action methods.
        public List<Hat> GetAllHats()
        {
            return _hats;
        }

        // GET /api/hats/styles/1 -> all open backed hats
        [HttpGet("styles/{style}")]
        public IEnumerable<Hat> GetHatsByStyle(HatStyle style)
        {
            var matches = _hats.Where(hat => hat.Style == style);

            return matches;
        }

        [HttpPost]
        public void AddAHat(Hat newHat)
        {
            _hats.Add(newHat);
        }

    }
}

// Controllers, in a general sense, are classes that have the specific purpose of processing an incoming request,
// calling any additional logic, and returning a response to that request.
// You can think of Controllers as an "orchestration layer" that will be responsible for
// accessing code in other parts of your application and handling the result of that code.
