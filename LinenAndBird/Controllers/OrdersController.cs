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
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        BirdRepository _birdRepository;
        HatRepository _hatRepository;
        OrdersRepository _orderRepository;

        public OrdersController()
        {
            _birdRepository = new BirdRepository();
            _hatRepository = new HatRepository();
            _orderRepository = new OrdersRepository();
        }


        [HttpPost]
        public IActionResult CreateOrder(CreateOrderCommand command)
        {
            var hatToOrder = _hatRepository.GetById(command.HatId);
            var birdToOrder = _birdRepository.GetById(command.BirdId);

            if (hatToOrder == null)
                return NotFound("There was no matching hat in the database.");

            if (birdToOrder == null)
                return NotFound("There was no matching bird in the database");

            var order = new Order
            {
                Bird = birdToOrder,
                Hat = hatToOrder,
                Price = command.Price
            };

            _orderRepository.Add(order);

            return Created($"/api/orders/{order.Id}", order);
        }
    }
}
