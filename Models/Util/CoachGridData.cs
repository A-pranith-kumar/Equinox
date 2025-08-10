// Models/Util/CoachGridData.cs
namespace Equinox.Models.Util
{
    public class CoachGridData : GridData
    {
        public CoachGridData() => SortField = nameof(DomainModels.User.Name);
        public bool IsSortByName => SortField.Equals(nameof(DomainModels.User.Name), System.StringComparison.OrdinalIgnoreCase);
        public bool IsSortById   => SortField.Equals(nameof(DomainModels.User.UserId), System.StringComparison.OrdinalIgnoreCase);
    }
}
