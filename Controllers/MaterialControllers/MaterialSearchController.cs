using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using static WebApiDB.Helpers.LevenshteinDistance;

namespace WebApiDB.Controllers.MaterialControllers
{
    public partial class MaterialController
    {
        /// <summary>
        /// Implements fuzzy search material by Texture
        /// </summary>
        /// <param name="searchString"> Строка для поиска</param>
        /// <returns>Dealer</returns>
        /// <response code="200">Material retrieved</response>
        /// <response code="404">Material not found</response>
        /// <response code="500">Oops! Can't lookup your Material right now</response>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(Material), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get([FromQuery] string searchString)
        {
            var materials = _materialRepository.GetAll().Result;
            var matches = new List<Material>();
            foreach (var material in materials)
            {
                if (Distance(material.Texture, searchString) <= 2)
                { matches.Add(material); continue; }


            }
            return Ok(matches);
        }
    }
}
