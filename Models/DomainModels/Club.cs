using System.ComponentModel.DataAnnotations;

namespace Equinox.Models.DomainModels
{
    public class Club
    {
        public int ClubId { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Only letters, numbers, and spaces allowed.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Phone number must be in format 000-000-0000")]
        public string PhoneNumber { get; set; }
    }
}
