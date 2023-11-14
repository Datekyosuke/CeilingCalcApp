using AutoMapper;
using CeilingCalc.Data.DTO_OrderDetail;
using CeilingCalc.Interfaces;
using CeilingCalc.Models;
using FluentValidation;
using FluentValidation.Results;
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
    }
}
