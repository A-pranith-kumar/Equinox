using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public List<EquinoxClass> EquinoxClasses { get; set; }
    }
}