using Equinox.Models.DomainModels; // ✅ Needed for ClassCategory

namespace Equinox.Models.Util
{
    public class ClassCategoryGridData : GridData
    {
        public ClassCategoryGridData() =>
            SortField = nameof(ClassCategory.Name);

        public bool IsSortByName =>
            SortField.Equals(nameof(ClassCategory.Name), System.StringComparison.OrdinalIgnoreCase);

        public bool IsSortById =>
            SortField.Equals(nameof(ClassCategory.ClassCategoryId), System.StringComparison.OrdinalIgnoreCase);
    }
}
