using System.Collections.Generic;

namespace Equinox.Models
{
    public class EquinoxFilterViewModel
    {
        public List<EquinoxClass> EquinoxClasses { get; set; } = new();   // ✅ Matches controller
        public List<Club> Clubs { get; set; } = new();
        public List<ClassCategory> Categories { get; set; } = new();

        public int? SelectedClubId { get; set; }
        public int? SelectedCategoryId { get; set; }
    }
}
