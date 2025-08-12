using System;
using System.Collections.Generic;

namespace Equinox.Models.ViewModels
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();
        public int Page { get; set; } = 1;       // 1-based
        public int PageSize { get; set; } = 4;   // default 4 per page
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPrev => Page > 1;
        public bool HasNext => Page < TotalPages;
        public int FirstItemNumber => TotalCount == 0 ? 0 : (Page - 1) * PageSize + 1;
        public int LastItemNumber  => Math.Min(Page * PageSize, TotalCount);
    }
}
