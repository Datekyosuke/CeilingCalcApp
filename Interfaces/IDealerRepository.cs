using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Interfaces
{
    public interface IDealerRepository 
    {
        public IEnumerable<Dealer> GetAll();
        public Task Delete(Dealer dealer);
        public Task Put(Dealer oldClient, Dealer dealer);
        public Task JsonPatchWithModelState(Dealer dealer,
         JsonPatchDocument<Dealer> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Dealer dealer);
        public Dealer Get(int id);

        public Task Patch(Dealer oldClient, Dealer dealer);

    }
}
