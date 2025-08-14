using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equinox.Models.DomainModels
{
    public class EquinoxClass
    {
        public int EquinoxClassId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ClassPicture { get; set; } = string.Empty;

        public string ClassDay { get; set; } = string.Empty;

        public string Time { get; set; } = string.Empty;

        // Foreign Key to ClassCategory (required via non-nullable FK)
        [Required]
        public int ClassCategoryId { get; set; }
        public ClassCategory ClassCategory { get; set; }   // no initializer

        // Foreign Key to User (Coach)
        [Required]
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }
        public User Coach { get; set; }                    // no initializer

        // Foreign Key to Club
        [Required]
        public int ClubId { get; set; }
        public Club Club { get; set; }                     // no initializer
    }
}
