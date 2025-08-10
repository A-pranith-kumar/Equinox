using System.ComponentModel.DataAnnotations;

namespace Equinox.Models.DomainModels
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int EquinoxClassId { get; set; }
        public EquinoxClass EquinoxClass { get; set; }
    }
}