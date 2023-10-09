using FuzzySharp;
using System.Reflection;

namespace WebApiDB.Helpers
{
    public static class SearchHelper
    {
        public static List<T> Search<T>(List<T> data, string searchString, string[] searchProperty)
        {
            var matches = new List<T>();

            if (searchString is not null)
            {
                foreach (var entity in data)
                {
                    var flag = searchString.Split(' ').Count();
                    foreach (var searchStr in searchString.Split(' '))
                    {
                        foreach(var searchPar in searchProperty) 
                        {
                            var property = typeof(T).GetProperty(searchPar, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);

                            if (Fuzz.PartialRatio(property.GetValue(entity).ToString().ToLower(), searchStr.ToLower()) >= 70)
                            { flag--; continue; }
                        }
                    }
                    if (flag == 0)
                        matches.Add(entity);
                }
                
            }
            
            return matches;
        }
    }
}
