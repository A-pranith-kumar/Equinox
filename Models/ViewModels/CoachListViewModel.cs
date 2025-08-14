using System.Collections.Generic;
using Equinox.Models.DomainModels;
using Equinox.Models.Util;

namespace Equinox.Models.ViewModels
{
    public class CoachListViewModel
    {
        public IEnumerable<User> Coaches { get; set; } = new List<User>();
        public CoachGridData CurrentRoute { get; set; } = new CoachGridData();
        public int TotalPages { get; set; }
    }
}
