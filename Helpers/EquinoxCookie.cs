using Microsoft.AspNetCore.Http;

namespace Equinox.Helpers
{
    public class EquinoxCookie
    {
        private readonly IHttpContextAccessor _accessor;

        public EquinoxCookie(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void SetBookingId(int bookingId)
        {
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7), // Optional: persistent cookie
                HttpOnly = true,
                IsEssential = true
            };

            _accessor.HttpContext?.Response?.Cookies.Append("BookingId", bookingId.ToString(), options);
        }

        public int? GetBookingId()
        {
            var cookie = _accessor.HttpContext?.Request?.Cookies["BookingId"];
            return int.TryParse(cookie, out int val) ? val : null;
        }

        public void RemoveBookingId()
        {
            _accessor.HttpContext?.Response?.Cookies.Delete("BookingId");
        }
    }
}
