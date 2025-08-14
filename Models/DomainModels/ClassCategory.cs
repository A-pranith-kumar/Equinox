using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Equinox.Models.DomainModels
{
    public class ClassCategory
    {
        public int ClassCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(60, ErrorMessage = "Name must be 60 characters or less.")]
        [RegularExpression(@"^[a-zA-Z0-9 \-&']+$", ErrorMessage = "Name may contain letters, numbers, spaces, and - & ' characters.")]
        [Display(Name = "Category Name")]
        // points to /Admin/Validation/CheckCategoryName
        [Remote("CheckCategoryName", "Validation", areaName: "Admin", AdditionalFields = nameof(ClassCategoryId))]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Image")]
        public string? Image { get; set; }
    }
}
