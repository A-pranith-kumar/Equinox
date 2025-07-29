using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime DOB { get; set; }

        public bool IsCoach { get; set; }

        public List<EquinoxClass> EquinoxClasses { get; set; }
    }
}