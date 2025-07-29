using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equinox.Models
{
    public class EquinoxClass
    {
        public int EquinoxClassId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ClassPicture { get; set; } = string.Empty;

        public string ClassDay { get; set; } = string.Empty;

        public string Time { get; set; } = string.Empty;

        // Foreign Key to ClassCategory
        public int ClassCategoryId { get; set; }
        public ClassCategory ClassCategory { get; set; } = default!;

        // Foreign Key to User (Coach)
        [ForeignKey("Coach")]
        public int CoachId { get; set; }
        public User Coach { get; set; } = default!;

        // Foreign Key to Club
        public int ClubId { get; set; }
        public Club Club { get; set; } = default!;
    }
}
