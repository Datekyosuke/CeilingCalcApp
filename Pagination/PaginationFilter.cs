namespace WebApiDB.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PaginationFilter(int pageNumber, int pageSize, int totalrecords)
        {
            if(pageNumber <= 0) 
            {
                this.PageNumber = 1;
                this.PageSize = totalrecords;
            } else
            { 
            this.PageNumber = pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            }
        }
    }
}
