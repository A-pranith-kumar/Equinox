using System.Collections.Generic;
using Equinox.Models.DomainModels;
using Equinox.Models.Util;

namespace Equinox.Models.ViewModels
{
    public class ClubListViewModel
    {
        public IEnumerable<Club> Clubs { get; set; } = new List<Club>();
        public ClubGridData CurrentRoute { get; set; } = new ClubGridData();
        public int TotalPages { get; set; }
    }
}
