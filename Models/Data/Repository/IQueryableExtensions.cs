// Models/Data/Repository/IQueryableExtensions.cs
using System.Linq;

namespace Equinox.Models.Data.Repository
{
    public static class IQueryableExtensions
    {
        // Mirrors the chapter’s paging helper name/shape.
        public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0) return query;
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
