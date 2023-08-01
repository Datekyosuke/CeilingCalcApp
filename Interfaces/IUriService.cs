using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
