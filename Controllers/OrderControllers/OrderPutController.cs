using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Making changes to one oder record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Properties can take Order field values:
        /// 
        ///     
        ///     
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="order"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Order order)
        {
            var oldOrder = _orderRepository.GetAsync(id).Result;
            if (oldOrder == null)
                return NotFound();
            // TODO Validation order!

            //if (dealer.FirstName.Length > 50)
            //    return BadRequest("FirstName cannot be more than 50 characters");
            //if (dealer.Telephone < 10000000000 || dealer.Telephone > 99999999999)
            //{
            //    return BadRequest("Invalid phone number. Must contain 10 digits!");
            //}
            //if (dealer.LastName.Length > 50 || dealer.LastName.Length < 2)
            //    return BadRequest("LastName cannot be more than 50 and less than 2 characters");
            //if (dealer.Debts > float.MaxValue || dealer.Debts < float.MinValue)
            //    return BadRequest("Wrong debts! Too big (small) number");
            //if (dealer.City.Length > 50 || dealer.City.Length < 2)
            //    return BadRequest("City cannot be more than 50 and less than 2 characters");

            await _orderRepository.Put(oldOrder, order);


            return Ok("Order changed!");
        }
    }
}
