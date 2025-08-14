// Models/Util/GridData.cs
using System.Collections.Generic;

namespace Equinox.Models.Util
{
    public abstract class GridData
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string SortField { get; set; } = "";
        public string SortDirection { get; set; } = "asc"; // "asc" | "desc"

        public int GetTotalPages(int count) => (count + PageSize - 1) / PageSize;

        public void SetSortAndDirection(string newSortField, GridData current)
        {
            if (current.SortField.Equals(newSortField, System.StringComparison.OrdinalIgnoreCase)
                && current.SortDirection == "asc")
                SortDirection = "desc";
            else
                SortDirection = "asc";

            SortField = newSortField;
        }

        public GridData Clone() => (GridData)MemberwiseClone();

        public virtual Dictionary<string, string> ToDictionary() => new()
        {
            { nameof(PageNumber), PageNumber.ToString() },
            { nameof(PageSize), PageSize.ToString() },
            { nameof(SortField), SortField ?? "" },
            { nameof(SortDirection), SortDirection ?? "asc" },
        };
    }
}
