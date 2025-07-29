using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<EquinoxClass> EquinoxClasses { get; set; }
    }
}