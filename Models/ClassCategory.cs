using System.ComponentModel.DataAnnotations;

namespace Equinox.Models
{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Alphanumeric only")]
        public required string Name { get; set; }

        // ✅ Add this property to fix the error
        public string? Image { get; set; }
    }
}