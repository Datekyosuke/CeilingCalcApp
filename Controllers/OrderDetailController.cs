using AutoMapper;
using CeilingCalc.Data.DTO_OrderDetail;
using CeilingCalc.Interfaces;
using CeilingCalc.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Repository;

namespace CeilingCalc.Controllers
{
    [Route("/api/OrderDetailController")]
    [ApiController]
    public class OrderDetailController : Controller
    {
        private IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;
        private IValidator<OrderDetail> _validatorOrderDetail;


        public OrderDetailController(IOrderDetailRepository orderDetailRepository, IMapper mapper, IValidator<OrderDetail> validatorOrderDetail)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _validatorOrderDetail = validatorOrderDetail;

        }
        [HttpGet()]

        /// <summary>
        /// Returns a paginated, sorted and ranged list of orders. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of oreders.
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "dillerFullName": "string"
        ///     "dateOrder": "date", regured, auto time
        ///     "OperatorId": id who added the order, integer
        ///     "Sum": full sum this order, float
        ///     "status": "string",  work/not work/on calculation etc    
        /// Sort 
        /// 
        ///     asc - ascending
        ///     desc - descending
        ///     else without sorting
        ///     
        ///  min  - search for sum from
        /// 
        ///  max - search for sum up
        /// </remarks>
        /// <returns>Page list orders</returns>
        /// <response code="200">Orders retrieved</response>
        ///  <response code="400">Wrong request body</response>
        public IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var trimSearchString = searchString?.Trim();
            var pagedReponse = _orderDetailRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString, route).Result;
            return Ok(pagedReponse);
        }


        [HttpGet("{id}")]
        /// <summary>
        /// Returns order by Id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order</returns>
        /// <response code="200">Order retrieved</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't lookup your Order right now</response>
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var orderDetail = _mapper.Map<OrderDetailDTO>(_orderDetailRepository.GetAsync(id).Result);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return Ok(new Response<OrderDetailDTO>(orderDetail));
        }

        [HttpPost]
        /// <summary>
        /// Create new order detail
        /// </summary>
        /// <param name="dealer">All fields of the order, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New order</returns>
        /// <response code="200">Order created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] OrderDetailDTO orderDetailDTO)
        {

            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);
            ValidationResult validationResult = await _validatorOrderDetail.ValidateAsync(orderDetail);

            if (validationResult.IsValid)
            {
                await _orderDetailRepository.Post(orderDetail);
                return Ok("OrderDetail created!");
            }

            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpDelete("{id}")]
        /// <summary>
        /// Removes order by id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Order removed</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't remove your Order right now</response>
        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = _orderDetailRepository.GetAsync(id).Result;

            if (orderDetail != null)
            {
                await _orderDetailRepository.Delete(orderDetail);
            }
            else return NotFound();
            return Ok("Order deleted!");
        }

        [HttpPatch]
        /// <summary>
        /// Making changes to one order record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields order:
        ///
        ///  
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="order"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        public async Task<ActionResult> Patch(int id, [FromBody] OrderDetailDTO orderDetailDTO)
        {
            var oldOrderDetail = _orderDetailRepository.GetAsync(id).Result;

            if (oldOrderDetail == null)
                return NotFound();
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDTO);

            ValidationResult validationResult = await _validatorOrderDetail.ValidateAsync(orderDetail);

            if (validationResult.IsValid)
            {
                orderDetail.Id = id;
                if (orderDetail.OrderId == 0)
                    orderDetail.OrderId = oldOrderDetail.Order.Id;
                if (orderDetail.MaterialId == 0)
                    orderDetail.MaterialId = oldOrderDetail.Material.Id;
                if (orderDetail.Count == 0)
                    orderDetail.Count = oldOrderDetail.Count;
                if (orderDetail.Sum == 0)
                    orderDetail.Sum = oldOrderDetail.Sum;
                if (orderDetail.Price == 0)
                    orderDetail.Price = oldOrderDetail.Price;

                await _orderDetailRepository.Patch(oldOrderDetail, orderDetail);
                return Ok("Order detail changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
        [HttpPut]
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
        public async Task<ActionResult> Put(int id, [FromBody] OrderDetailDTO orderDetailDTO)
        {
            var oldOrderDetail = _orderDetailRepository.GetAsync(id).Result;
            if (oldOrderDetail == null)
                return NotFound();
            var order = _mapper.Map<OrderDetail>(orderDetailDTO);

            ValidationResult validationResult = await _validatorOrderDetail.ValidateAsync(order);

            if (validationResult.IsValid)
            {
                await _orderDetailRepository.Put(oldOrderDetail, order);
                return Ok("Order detail changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }


        [HttpPatch("PatchJson")]
        /// <summary>
        /// Making changes to one or more dealer fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "Status",
        ///        "value": "In work"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the Status field of the selected order by id to "In work"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        /// Properties can take Order field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "dealerId": integer, required
        ///     "DateOrder": "date time", not null, in format DateTime(2015, 7, 20, 18, 30, 25); // год - месяц - день - час - минута - секунда
        ///     "OperatorId": integer, who added order  
        ///     "Sum": float, full sum of order
        ///     "Status": "string",  status of order. in work, in calculation etc    
        ///     
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<OrderDetail> patchDoc)
        {
            //TODO: validation order!

            if (patchDoc.Operations[0].path.ToLower() == "id")
                return BadRequest("id cannot be changes");

            if (patchDoc.Operations[0].path.ToLower() == "dateorder")
            {
                if (!DateTime.TryParse(patchDoc.Operations[0].value.ToString(), out DateTime dateTime))
                    return BadRequest("Wrong date time! Must be a number");
                else if (dateTime > DateTime.Now)
                    return BadRequest("Wrong date! Not future");
            }

            if (patchDoc.Operations[0].path.ToLower() == "sum")
            {
                {
                    if (patchDoc.Operations[0].value == "")
                        patchDoc.Operations[0].value = 0;
                    if (!float.TryParse(patchDoc.Operations[0].value.ToString(), out float sum))
                        return BadRequest("Wrong debts! Must be a number");
                    else if (sum < float.MinValue || sum > float.MaxValue)
                        return BadRequest("Wrong sum! Too big (small) number");
                }

            }
            if (patchDoc.Operations[0].path.ToLower() == "status" && (patchDoc.Operations[0].value.ToString().Length > 50))
                return BadRequest("Status cannot be more than 50 and less than 2 characters");

            if (patchDoc != null)
            {
                var orderDTO = _orderDetailRepository.GetAsync(id).Result;
                /* var order = _mapper.Map<Order>(orderDTO);*/
                await _orderDetailRepository.JsonPatchWithModelState(orderDTO, patchDoc, ModelState);
                return new ObjectResult(orderDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
