using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Repository;

namespace CeilingCalc.Controllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/DealerController")]
    [ApiController]

    public class DealerController : Controller
    {

        private IDealerRepository _dealerRepository;
        private readonly IMapper _mapper;
        private IValidator<Dealer> _validatorDealer;


        public DealerController(IDealerRepository dealerRepository, IMapper mapper, IValidator<Dealer> validatorDealer)
        {
            _dealerRepository = dealerRepository;
            _mapper = mapper;
            _validatorDealer = validatorDealer;

        }
        /// <summary>
        /// Returns a paginated, sorted and ranged list of dealers. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of dealers.
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required    
        /// Sort 
        /// 
        ///     asc - ascending
        ///     desc - descending
        ///     else without sorting
        ///     
        ///  min  - search for debts from
        /// 
        ///  max - search for debts up
        /// </remarks>
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [AllowAnonymous]
        [HttpGet()]
        public virtual IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var trimSearchString = searchString?.Trim();
            var pagedReponse = _dealerRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString, route).Result;
            return Ok(pagedReponse);
        }

        /// <summary>
        /// Returns dealer by Id
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <returns>Dealer</returns>
        /// <response code="200">Dealer retrieved</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't lookup your Dealer right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Dealer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var dealer = _dealerRepository.GetAsync(id).Result;

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(new Response<Dealer>(dealer));
        }

        /// <summary>
        /// Removes dealer by id
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Dealer removed</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't remove your Dealer right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dealer = _dealerRepository.GetAsync(id).Result;

            if (dealer != null)
            {
                await _dealerRepository.Delete(dealer);
            }
            else return NotFound();
            return Ok("Dealer deleted!");
        }

        /// <summary>
        /// Making changes to one dealer record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] DealerDTOGet dTOdealer)
        {
            var dealer = _mapper.Map<Dealer>(dTOdealer);
            var oldClient = _dealerRepository.GetAsync(id).Result;

            if (oldClient == null)
                return NotFound();

            var validationResult = _validatorDealer.Validate(dealer);
            if (validationResult.IsValid)
            {
                if (dealer.FirstName == "string")
                    dealer.FirstName = oldClient.FirstName;
                if (dealer.LastName == "string")
                    dealer.LastName = oldClient.LastName;
                if (dealer.Telephone == 0)
                    dealer.Telephone = oldClient.Telephone;
                if (dealer.Debts == 0)
                    dealer.Debts = oldClient.Debts;
                if (dealer.City == "string")
                    dealer.City = oldClient.City;

                await _dealerRepository.Patch(oldClient, dealer);

                return Ok("Dealer changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        /// <summary>
        /// Making changes to one or more dealer fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "FirstName",
        ///        "value": "Barry"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the FirstName field of the selected dealer by id to "Barry"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required    
        ///     
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("PatchJson")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<Dealer> patchDoc)
        {
            if (patchDoc != null)
            {
                var dealer = _dealerRepository.GetAsync(id).Result;
                patchDoc.ApplyTo(dealer);
                ValidationResult validationResult = await _validatorDealer.ValidateAsync(dealer);

                if (validationResult.IsValid)
                {
                    await _dealerRepository.JsonPatchWithModelState();
                    return new ObjectResult(dealer);
                }

                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] DealerDTOGet dealerDtoGet)
        {
            var dealer = _mapper.Map<Dealer>(dealerDtoGet);
            var validationResult = _validatorDealer.Validate(dealer);
            if (validationResult.IsValid)
            {

                await _dealerRepository.Post(dealer);
                return Ok("Dealer created!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        /// <summary>
        /// Making changes to one dealer record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required    
        ///     
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] DealerDTOGet dTOdealer)
        {
            var dealer = _mapper.Map<Dealer>(dTOdealer);
            var oldClient = _dealerRepository.GetAsync(id).Result;

            if (oldClient == null)
                return NotFound();

            var validationResult = _validatorDealer.Validate(dealer);

            if (validationResult.IsValid)
            {
                await _dealerRepository.Put(oldClient, dealer);
                return Ok("Dealer changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
