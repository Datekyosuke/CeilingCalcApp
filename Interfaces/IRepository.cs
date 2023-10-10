using Microsoft.AspNetCore.JsonPatch;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApiDB.Models;

namespace WebApiDB.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task Delete(T entity);
        public Task Put(T oldEntity, T entity);
        public Task JsonPatchWithModelState(T entity,
         JsonPatchDocument<T> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(T entity);
        public Task<T> GetAsync(int id);

        public Task Patch(T oldEntity, T entity);
        public Task<IEnumerable<T>> Get(string propertyCamelCase, string sort);
    }
}
