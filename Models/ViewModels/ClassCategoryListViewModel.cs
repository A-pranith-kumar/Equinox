using System.Collections.Generic;
using Equinox.Models.DomainModels;
using Equinox.Models.Util;

namespace Equinox.Models.ViewModels
{
    public class ClassCategoryListViewModel
    {
        public IEnumerable<ClassCategory> Categories { get; set; } = new List<ClassCategory>();
        public ClassCategoryGridData CurrentRoute { get; set; } = new ClassCategoryGridData();
        public int TotalPages { get; set; }
    }
}
