using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Returns order by Id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order</returns>
        /// <response code="200">Order retrieved</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't lookup your Order right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var order = _orderRepository.GetAsync(id).Result;

            if (order == null)
            {
                return NotFound();
            }

            return Ok(new Response<Order>(order));
        }
    }
}
