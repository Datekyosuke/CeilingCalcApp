using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Interfaces
{
    public interface IDealerRepository 
    {
        public IEnumerable<Dealer> GetAll();
        public Task<IActionResult> Delete();
        public Task<ActionResult> Put(int id, Dealer dealer);
        public Task<IActionResult> JsonPatchWithModelState(int id,
         JsonPatchDocument<Dealer> patchDoc);
        public Task Post(Dealer dealer);
        public Dealer Get(int id);

    }
}
