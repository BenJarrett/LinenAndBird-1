using LinenAndBird.DataAccess;
using LinenAndBird.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LinenAndBird.Controllers
{
    [Route("api/birds")]
    [ApiController]
    public class BirdController : ControllerBase
    {
        BirdRepository _repo;

        public BirdController()
        {
            _repo = new BirdRepository();
        }

        [HttpGet]
        public IActionResult GetAllBirds()
        {
            return Ok(_repo.GetAll());
        }

        [HttpPost]
        public IActionResult AddBird(Bird newBird)
        {
            if (string.IsNullOrEmpty(newBird.Name) || string.IsNullOrEmpty(newBird.Color))
            {
                return BadRequest("Name and Color are required fields");
            }

            _repo.Add(newBird);

            return Created("/api/birds/1", newBird);
        }
    }
}
