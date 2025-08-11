using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Equinox.Models.Validation;

namespace Equinox.Models.DomainModels
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric.")]
        [Display(Name = "Name")]
        // point to /Admin/Validation/CheckUserName
        [Remote("CheckUserName", "Validation", areaName: "Admin", AdditionalFields = nameof(UserId))]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        // point to /Admin/Validation/CheckPhone
        [Remote("CheckPhone", "Validation", areaName: "Admin", AdditionalFields = nameof(UserId))]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        // point to /Admin/Validation/CheckEmail
        [Remote("CheckEmail", "Validation", areaName: "Admin", AdditionalFields = nameof(UserId))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [MinimumAge(8, 80, ErrorMessage = "Age must be between 8 and 80.")]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }

        [Display(Name = "Is Coach")]
        public bool IsCoach { get; set; } = false;
    }
}
