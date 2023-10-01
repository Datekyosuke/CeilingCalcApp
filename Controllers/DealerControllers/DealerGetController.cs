using FuzzySharp;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.DealerControllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/DealerController")]
    public class DealerController : Controller
    {
        private IDealerRepository _dealerRepository;
        public DealerController(IDealerRepository dealerRepository, IUriService uriService)
        {
            _dealerRepository = dealerRepository;
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
        ///     Id
        ///     FirstName
        ///     LastName
        ///     Telephone
        ///     Debts
        ///     City
        /// Sort is enum
        /// 
        ///     0 ASc
        ///     1 Desc
        ///     null without sorting
        ///     
        ///  Min  - search for debts from
        /// 
        ///  Max - search for debts up
        /// </remarks>
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum"); 
            var route = Request.Path.Value;
            var totalRecords = _dealerRepository.Count();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, totalRecords);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var entities = _dealerRepository.GetAllAsync(validFilter, expression, sort, ranges).Result;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Dealer>(entities, validFilter, totalRecords, _uriService, route);
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
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Dealer dealer)
        {
            var validation = ValidationDealer.DealerValidation(dealer);
            if (!validation.Item1)
            {
                return BadRequest(validation.Item2);
            }

            await _dealerRepository.Post(dealer);
            return Ok("Dealer created!");
        }

        /// <summary>
        /// Making changes to one dealer record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Fields dealer:
        /// 
        ///    "Id": can't be changed, integer
        ///    
        ///     "FirstName": "string", required
        ///     
        ///     "LastName": "string", may be null
        ///     
        ///     "Telephone": 11 digit, integer
        ///     
        ///     "Debts": 0, integer
        ///     
        ///     "City": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Dealer dealer)
        {
            var oldClient = _dealerRepository.GetAsync(id).Result;
            if (oldClient == null)
                return NotFound();

            if (dealer.FirstName.Length > 50)
                return BadRequest("FirstName cannot be more than 50 characters");
            if (dealer.Telephone < 10000000000 || dealer.Telephone > 99999999999)
            {
                return BadRequest("Invalid phone number. Must contain 10 digits!");
            }
            if (dealer.LastName.Length > 50 || dealer.LastName.Length < 2)
                return BadRequest("LastName cannot be more than 50 and less than 2 characters");
            if (dealer.Debts > float.MaxValue || dealer.Debts < float.MinValue)
                return BadRequest("Wrong debts! Too big (small) number");
            if (dealer.City.Length > 50 || dealer.City.Length < 2)
                return BadRequest("City cannot be more than 50 and less than 2 characters");

            await _dealerRepository.Put(oldClient, dealer);


            return Ok("Dealer changed!");
        }

        /// <summary>
        /// Making changes to one dealer record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "Id": can't be changed, integer
        ///     "FirstName": "string", required
        ///     "LastName": "string", may be null
        ///     "Telephone": 11 digit, integer
        ///     "Debts": 0, integer
        ///     "City": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] Dealer dealer)
        {
            var oldClient = _dealerRepository.GetAsync(id).Result;

            if (oldClient == null)
                return NotFound();

            var validation = ValidationDealer.DealerValidation(dealer);
            if (!validation.Item1)
            {
                return BadRequest(validation.Item2);
            }
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
        ///     Fields dealer:
        ///     
        ///     "Id": can't be changed, integer
        ///     "FirstName": "string", may be null
        ///     "LastName": "string", required
        ///     "Telephone": 11 digit, integer
        ///     "Debts": 0, integer
        ///     "City": "string",  required
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

            if (patchDoc.Operations[0].path == "LastName" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("LastName cannot be more than 50 and less than 2 characters");

            if (patchDoc.Operations[0].path == "FirstName" && patchDoc.Operations[0].value.ToString().Length > 50)
                return BadRequest("FirstName cannot be more than 50 characters");

            if (patchDoc.Operations[0].path == "Telephone")
            {
                long.TryParse(patchDoc.Operations[0].value.ToString(), out long telephone);
                if (telephone < 10000000000 || telephone > 99999999999)
                    return BadRequest("Invalid telephone. Must contain 11 digits!");
            }

            if (patchDoc.Operations[0].path == "Debts")
            {
                {
                    if (!float.TryParse(patchDoc.Operations[0].value.ToString(), out float debts))
                        return BadRequest("Wrong debts! Must be a number");
                    else if (debts < float.MinValue || debts > float.MaxValue)
                        return BadRequest("Wrong debts! Too big (small) number");
                }

            }
            if (patchDoc.Operations[0].path == "City" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("City cannot be more than 50 and less than 2 characters");

            if (patchDoc != null)
            {
                var customer = _dealerRepository.GetAsync(id).Result;
                await _dealerRepository.JsonPatchWithModelState(customer, patchDoc, ModelState);


                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
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

    }
}
