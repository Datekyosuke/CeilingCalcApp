using FuzzySharp;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly IUriService _uriService;
        DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
            _dbSet = context.Set<TEntity>();
        }


        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }
       


        public async Task JsonPatchWithModelState(TEntity entity, JsonPatchDocument<TEntity> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            patchDoc.ApplyTo(entity, modelState);
            await _context.SaveChangesAsync();
        }

        public async Task Post(TEntity entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Put(TEntity oldEntity, TEntity entity)
        {
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Patch(TEntity oldEntity, TEntity entity)
        {
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<TEntity>>> GetAllAsync(PaginationFilter validFilter, string property, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var sortEntity =
                        sort == "Asc" ?
                        _dbSet
                        .Select(x => x)
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "Desc" ?
                        _dbSet
                       .Select(x => x)
                       .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _dbSet
                        .Select(x => x);

           /* var matches = new List<TEntity>();
            if (searchString is not null)
            {
                foreach (var str in searchString.Split(' '))
                    foreach (var entity in sortEntity)
                    {
                        if (Fuzz.PartialRatio(dealer.LastName.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(entity); continue; }
                        if (Fuzz.PartialRatio(dealer.FirstName.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(entity); continue; }
                        if (Fuzz.PartialRatio(dealer.City.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(entity); continue; }

                    }
                totalRecords = matches.Distinct().Count();
                var sortedSearchEntities = matches                        
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return PaginationHelper.CreatePagedReponse<TEntity>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            }*/
            totalRecords = sortEntity.Count();
            var sortedEntities = sortEntity                       
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToList();
            return PaginationHelper.CreatePagedReponse<TEntity>(sortedEntities, validFilter, totalRecords, _uriService, route);


        }
    }
}
