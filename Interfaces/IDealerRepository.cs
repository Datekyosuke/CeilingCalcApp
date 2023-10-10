using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IDealerRepository 
    {
        //public Task<List<Dealer>> GetAllAsync();
        public Task Delete(Dealer dealer);
        public Task Put(Dealer oldClient, Dealer dealer);
        public Task JsonPatchWithModelState(Dealer dealer,
         JsonPatchDocument<Dealer> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Dealer dealer);
        public Task<Dealer> GetAsync(int id);

        public Task Patch(Dealer oldClient, Dealer dealer);

        public int Count();
        public Task<IEnumerable<Dealer>> Get(string propertyCamelCase, string sort);
    }
}
