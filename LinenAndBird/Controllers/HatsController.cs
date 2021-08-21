using LinenAndBird.DataAccess;
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
    // ASP.NET is reading this metadata
    [ApiController] // an api controller, so it returns json or xml; This attribute indicates that the controller responds to web API requests. 
    public class HatsController : ControllerBase // A base class for an MVC controller without view support.
    {
        HatRepository _repo; // field has type + name
        public HatsController()
        {
            _repo = new HatRepository();
        }
        [HttpGet] // attributes can be used to configure the behavior of web API controllers and action methods.
        public List<Hat> GetAllHats()
        {
            var repo = new HatRepository();
            return repo.GetAll();
        }

        // GET /api/hats/styles/1 -> all open backed hats
        [HttpGet("styles/{style}")] // how should this method be exposed & executed
        // all additive
        public IEnumerable<Hat> GetHatsByStyle(HatStyle style) // IEnum is just to loop over; restrictive permissions
        {
            return _repo.GetByStyle(style);
        }

        [HttpPost]
        public void AddAHat(Hat newHat)
        {
            var repo = new HatRepository();
            repo.Add(newHat);
        }

    }
}

// Controllers, in a general sense, are classes that have the specific purpose of processing an incoming request,
// calling any additional logic, and returning a response to that request.
// You can think of Controllers as an "orchestration layer" that will be responsible for
// accessing code in other parts of your application and handling the result of that code.
