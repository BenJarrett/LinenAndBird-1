using LinenAndBird.DataAccess;
using LinenAndBird.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace LinenAndBird.Controllers
{
    [Route("api/birds")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        BirdRepository _repo;

        // DEPENDENCY INJECTION //
        public BirdController(BirdRepository repo) // This is asking asp.net for the application congiuration // In order to construct this controller, you must supply us with a configuration // 
        {
            _repo = repo;
        }

        // DEPENDENCY INJECTION //

        [HttpGet]
        public IActionResult GetAllBirds()
        {
            return Ok(_repo.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetBirdById(Guid id)
        {
            var bird = _repo.GetById(id);

            if (bird == null)
            {
                return NotFound($"No bird with the id {id} was found.");
            }

            return Ok(bird);
        }

        [HttpPost]
        public IActionResult AddBird(Bird newBird)
        {
            if (string.IsNullOrEmpty(newBird.Name) || string.IsNullOrEmpty(newBird.Color))
            {
                return BadRequest("Name and Color are required fields");
            }

            _repo.Add(newBird);

            return Created($"/api/birds/{newBird.Id}", newBird); // The location of the new bird that is created //
        }

        // api/birds/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBird(Guid id)
        {
            _repo.Remove(id);

            return Ok("Successfully Deleted Bird from Database");
        }

        // api/birds/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateBird(Guid id, Bird bird)
        {
            var birdToUpdate = _repo.GetById(id);

            if (birdToUpdate == null)
            {
                return NotFound($"Could not find bird with the Id {id} for updating");
            }

            var updatedBird = _repo.Update(id, bird);

            return Ok(updatedBird);

        }
    }
}

// LIST OF ADO.NET STEPS //
// 1. - Create and open a connection 
// 2. - Create command and set command text and, if applicable, set parameters
// 3. - Execute that command As reader, scaler, or not query. If you care about that data, you have to map that data from the relational side of things to the object side of things. Relational to C# objects. Usually in a While Loop or an If Statement // 
// 4. - Then return the results //