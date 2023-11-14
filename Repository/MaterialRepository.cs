using AutoMapper;
using CeilingCalc.Data.DTO_Material;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Context;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Servics;

namespace WebApiDB.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AplicationContext _context;
        private readonly IUriService _uriService;
        private IMapper _mapper;

        public MaterialRepository(AplicationContext context, IUriService uriService, IMapper mapper)
        {
            _context = context;
            _uriService = uriService;   
            _mapper = mapper;
        }

        public async Task Delete(Material materail)
        {
            _context.Materials.Remove(materail);
            await _context.SaveChangesAsync();
        }

        public async Task<Material> GetAsync(int id)
        {
            var materail = await _context.Materials.FirstOrDefaultAsync(p => p.Id == id);
            return materail;
        }
        public async Task<PagedResponse<List<MaterialDTO>>> GetAllAsync(PaginationFilter validFilter, string propertyCamelCase, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var firstChar = propertyCamelCase[0].ToString().ToUpper();
            var property = firstChar + propertyCamelCase.Substring(1);
            var sortDealers =
                        sort == "asc" ?
                        _context.Materials
                        .Select(x => new MaterialDTO
                        {
                            Id = x.Id,
                            Texture = x.Texture,
                            Color = x.Color,
                            Size = x.Size,
                            Price = x.Price,
                        })
                        .AsQueryable()
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "desc" ?
                        _context.Materials
                        .Select(x => new MaterialDTO
                        {
                            Id = x.Id,
                            Texture = x.Texture,
                            Color = x.Color,
                            Size = x.Size,
                            Price = x.Price,
                        })
                        .AsQueryable()
                       .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _context.Materials
                        .Select(x => new MaterialDTO
                        {
                            Id = x.Id,
                            Texture = x.Texture,
                            Color = x.Color,
                            Size = x.Size,
                            Price = x.Price,
                        })
                        .AsQueryable();



            if (searchString is not null)
            {
                string[] propertySearch = { "Texture", "Color" };
                var matches = SearchHelper.Search(sortDealers.ToList(), searchString, propertySearch);
                totalRecords = matches.Distinct().Count();


                var sortedSearchEntities = matches
                        .Where(x => x.Size >= ranges.Min && x.Size <= ranges.Max)
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return PaginationHelper.CreatePagedReponse<MaterialDTO>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            }
            totalRecords = sortDealers.Count();
            var sortedEntities = sortDealers
                       .Where(x => x.Size >= ranges.Min && x.Size <= ranges.Max)
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToList();
            return PaginationHelper.CreatePagedReponse<MaterialDTO>(sortedEntities, validFilter, totalRecords, _uriService, route);


        }

        public async Task JsonPatchWithModelState(Material material, JsonPatchDocument<Material> patchDoc, ModelStateDictionary modelState)
        {
            patchDoc.ApplyTo(material, modelState);
            await _context.SaveChangesAsync();
        }

        public async Task Patch(Material oldMaterial, Material material)
        {
            _context.Entry(oldMaterial).CurrentValues.SetValues(material);
            await _context.SaveChangesAsync();
        }

        public async Task Post(Material material)
        {
            _context.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task Put(Material oldMaterial, Material material)
        {
            _context.Entry(oldMaterial).CurrentValues.SetValues(material);
            await _context.SaveChangesAsync();
        }
    }
}
