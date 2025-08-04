
namespace Equinox.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
    }
}
