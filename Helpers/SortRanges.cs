using System.Reflection;
using WebApiDB.Pagination;

namespace WebApiDB.Helpers
{
    public static class SortRanges
    {
        public static List<T> SortEntitesRange<T>(IQueryable<T> data, NumericRanges ranges, string sortProperty, PaginationFilter validFilter)
        {
            var property = typeof(T).GetProperty(sortProperty, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
           
            var sortedEntities = data
                        .Where(x => float.Parse(property.GetValue(x).ToString()) >= ranges.Min && float.Parse(property.GetValue(x).ToString()) <= ranges.Max)
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
            return sortedEntities;
        }
    }
}