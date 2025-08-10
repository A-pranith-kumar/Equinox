using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models.DomainModels
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Name must be alphanumeric.")]
        [Display(Name = "Name")]
        [Remote(
            action: "VerifyName",
            controller: "User",
            areaName: "Admin",
            AdditionalFields = nameof(UserId),
            HttpMethod = "Post",
            ErrorMessage = "Name already exists."
        )]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        [Remote(
            action: "VerifyPhoneNumber",
            controller: "User",
            areaName: "Admin",
            AdditionalFields = nameof(UserId),
            HttpMethod = "Post",
            ErrorMessage = "Phone number already exists."
        )]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        [Remote(
            action: "VerifyEmail",
            controller: "User",
            areaName: "Admin",
            AdditionalFields = nameof(UserId),
            HttpMethod = "Post",
            ErrorMessage = "Email already exists."
        )]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [MinimumAge(8, 80)]
        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Is Coach")]
        public bool IsCoach { get; set; } = false;
    }
}
