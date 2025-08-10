using System.Collections.Generic;
using Equinox.Models.DomainModels; // ✅ Needed for EquinoxClass, Club, ClassCategory

namespace Equinox.Models.ViewModels
{
    public class EquinoxFilterViewModel
    {
        public List<EquinoxClass> EquinoxClasses { get; set; } = new();
        public List<Club> Clubs { get; set; } = new();
        public List<ClassCategory> Categories { get; set; } = new();

        public int? SelectedClubId { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
