﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly MaterialContext _context;

        public MaterialRepository(MaterialContext context)
        {
            _context = context;
        }
        public int Count()
        {
            return _context.Materials.Count();
        }

        public async Task Delete(Material materail)
        {
            _context.Materials.Remove(materail);
            await _context.SaveChangesAsync();
        }

        public Material Get(int id)
        {
            var materail = _context.Materials.SingleOrDefault(p => p.Id == id);
            return materail;
        }

        public List<Material> GetAll()
        {
            var pagedData = _context.Materials.ToList();
            return pagedData;
        }

        public List<Material> GetAll(PaginationFilter validFilter, string property, Sort sort, NumericRanges ranges)
        {
            if (sort == Sort.Asc)
            {
                var sortDealers = _context.Materials
                            .Select(x => x)
                            .OrderBy(x => EF.Property<object>(x, property));
                var sortEntities = from Material entity in sortDealers
                                   where entity.Price >= ranges.Min && entity.Price <= ranges.Max
                                   select entity;
                return sortEntities
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToList();
            }
            else
            {
                var sortDealers = _context.Materials
                           .Select(x => x)
                           .OrderByDescending(x => EF.Property<object>(x, property));
                var sortEntities = from Material entity in sortDealers
                                   where entity.Price >= ranges.Min && entity.Price <= ranges.Max
                                   select entity;
                return sortEntities
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToList();
            }
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