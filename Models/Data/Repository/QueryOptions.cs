// Models/Data/Repository/QueryOptions.cs
using System;
using System.Linq.Expressions;

namespace Equinox.Models.Data.Repository
{
    public class QueryOptions<T>
    {
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, bool>>? Where { get; set; }
        public string OrderByDirection { get; set; } = "asc";
        public int PageNumber { get; set; }        // 1-based
        public int PageSize { get; set; }

        private string[] _includes = Array.Empty<string>();
        public string Includes
        {
            set => _includes = (value ?? "").Replace(" ", "").Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        public string[] GetIncludes() => _includes;

        public bool HasWhere => Where != null;
        public bool HasOrderBy => OrderBy != null;
        public bool HasPaging => PageNumber > 0 && PageSize > 0;
    }
}
