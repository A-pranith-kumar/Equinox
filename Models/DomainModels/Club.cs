using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models.DomainModels
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(60, ErrorMessage = "Name must be 60 characters or less.")]
        [RegularExpression(@"^[a-zA-Z0-9 \-&']+$", ErrorMessage = "Name may contain letters, numbers, spaces, and - & ' characters.")]
        [Display(Name = "Club Name")]
        // points to /Admin/Validation/CheckClubName
        [Remote("CheckClubName", "Validation", areaName: "Admin", AdditionalFields = nameof(ClubId))]
        public string Name { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
