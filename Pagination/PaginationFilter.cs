using System.Text.Json.Serialization;

namespace WebApiDB.Pagination
{
    public class PaginationFilter
    {
        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            if(pageNumber <= 0) 
            {
                this.PageNumber = 1;
                this.PageSize = int.MaxValue;
            } else
            { 
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            }
        }
    }
}
