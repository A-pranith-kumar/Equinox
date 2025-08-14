// Models/Util/ClubGridData.cs
namespace Equinox.Models.Util
{
    public class ClubGridData : GridData
    {
        public ClubGridData() => SortField = nameof(DomainModels.Club.Name);
        public bool IsSortByName => SortField.Equals(nameof(DomainModels.Club.Name), System.StringComparison.OrdinalIgnoreCase);
        public bool IsSortById   => SortField.Equals(nameof(DomainModels.Club.ClubId), System.StringComparison.OrdinalIgnoreCase);
    }
}
