using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
        [Remote("VerifyPhone", "User", AdditionalFields = "UserId", ErrorMessage = "Phone already used")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [MinimumAge(18, ErrorMessage = "Must be at least 18 years old")]
        public DateTime DOB { get; set; }

        public bool IsCoach { get; set; }
    }
}
