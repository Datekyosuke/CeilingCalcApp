using AutoMapper;
using CeilingCalc.Data.DTO_Material;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Repository;

namespace CeilingCalc.Controllers
{
    [Route("/api/MaterailController")]
    [ApiController]
    public partial class MaterialController : Controller
    {
        private IMaterialRepository _materialRepository;
        private readonly IUriService _uriService;
        private IValidator<Material> _validatorMaterial;
        private IMapper _mapper;

        public MaterialController(IMaterialRepository materialRepository, IUriService uriService, IValidator<Material> validatorMaterial, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _uriService = uriService;
            _validatorMaterial = validatorMaterial;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns a paginated, sorted and ranged list of materials. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of materials.
        /// 
        /// Properties can take Material field values:
        /// 
        ///     Id
        ///     Texture
        ///     Color
        ///     Size
        ///     Price
        /// Sort is enum
        /// 
        ///     0 ASc
        ///     1 Desc
        ///     null without sorting
        ///     
        ///  Min  - search for price from
        /// 
        ///  Max - search for price up
        /// </remarks>
        /// <returns>Page list materials</returns>
        /// <response code="200">materials retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public virtual IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var trimSearchString = searchString?.Trim();
            var pagedReponse = _materialRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString).Result;
            return Ok(pagedReponse);
        }

        /// <summary>
        /// Returns material by Id
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Material</returns>
        /// <response code="200">Material retrieved</response>
        /// <response code="404">Material not found</response>
        /// <response code="500">Oops! Can't lookup your Material right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Material), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var material = _mapper.Map<MaterialDTO>(_materialRepository.GetAsync(id).Result);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(new Response<MaterialDTO>(material));
        }

        /// <summary>
        /// Removes material by id
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Material removed</response>
        /// <response code="404">Material not found</response>
        /// <response code="500">Oops! Can't remove your Material right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var material = _materialRepository.GetAsync(id).Result;

            if (material != null)
            {
                await _materialRepository.Delete(material);
            }
            else return NotFound();
            return Ok("Material deleted!");
        }

        /// <summary>
        /// Making changes to one material record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "Id": can't be changed, integer
        ///     "Texture": "string", required
        ///     "Color": "string", min 2 character
        ///     "Size": float, may be bull
        ///     "Price": float, may be bull
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="material"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no material for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] MaterialDTO materialDTO)
        {
            var material = _mapper.Map<Material>(materialDTO);
            var oldMaterial = _materialRepository.GetAsync(id).Result;

            if (oldMaterial == null)
                return NotFound();
            ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

            if (validationResult.IsValid)
            {

                if (material.Texture == "string")
                    material.Texture = oldMaterial.Texture;
                if (material.Color == "string")
                    material.Color = oldMaterial.Color;
                if (material.Size == 0)
                    material.Size = oldMaterial.Size;
                if (material.Price == 0)
                    material.Price = oldMaterial.Price;

                await _materialRepository.Patch(oldMaterial, material);

                return Ok("Material changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        /// <summary>
        /// Making changes to one or more material fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "Texture",
        ///        "value": "Mate"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the Texture field of the selected dealer by id to "Mate"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        ///     Fields materail:
        ///     
        ///     "Id": can't be changed, integer
        ///     "Texture": "string", required, min 3 character
        ///     "Color": "string", min 2 character
        ///     "Size": float  more 1 and less 6
        ///     "Price": float, 0
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no material for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("PatchJson")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<Material> patchDoc)
        {
            
            if (patchDoc != null)
            {
                var material = _materialRepository.GetAsync(id).Result;
                patchDoc.ApplyTo(material);
                ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

                if (validationResult.IsValid)
                {
                    await _materialRepository.JsonPatchWithModelState();
                    return new ObjectResult(material);
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
        /// Create new material
        /// </summary>
        /// <param name="material">All fields of the material, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New material</returns>
        /// <response code="200">Material created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MaterialDTO materialDTO)
        {
            var material = _mapper.Map<Material>(materialDTO);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

            if (validationResult.IsValid)
            {
                await _materialRepository.Post(material);
                return Ok("Material created!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        /// <summary>
        /// Making changes to one material record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Fields dealer:
        /// 
        ///    "Id": can't be changed, integer
        ///    
        ///     "Texture": "string", required, minimum 3 character
        ///     
        ///     "Color": "string", minimum 2 character
        ///     
        ///     "Size": float, Size should be between 1 and 6, may be null
        ///     
        ///     "Price": float, 0
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="material"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] MaterialDTO materialDTO)
        {
            var material = _mapper.Map<Material>(materialDTO);
            var oldMaterial = _materialRepository.GetAsync(id).Result;
            if (oldMaterial == null)
                return NotFound();
            ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

            if (validationResult.IsValid)
            {
                await _materialRepository.Put(oldMaterial, material);


                return Ok("Material changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
