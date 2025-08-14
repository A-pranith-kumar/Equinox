using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Equinox.Models.DomainModels
{
    public class Membership
    {
        public int MembershipId { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10000)]
        [Column(TypeName = "decimal(8,2)")] // OK if using SQL Server; harmless on SQLite
        public decimal Price { get; set; }
    }
}

