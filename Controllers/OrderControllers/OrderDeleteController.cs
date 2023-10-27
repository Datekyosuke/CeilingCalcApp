using Microsoft.AspNetCore.Mvc;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Removes order by id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Order removed</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't remove your Order right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = _orderRepository.GetAsync(id).Result;

            if (order != null)
            {
                await _orderRepository.Delete(order);
            }
            else return NotFound();
            return Ok("Order deleted!");
        }
    }
}
