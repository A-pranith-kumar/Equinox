using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Equinox.Models
{
    public static class SessionHelper
    {
        private const string BookingKey = "Bookings";

        public static void AddBooking(ISession session, int classId)
        {
            var bookings = GetBookings(session);
            if (!bookings.Contains(classId))
                bookings.Add(classId);

            session.SetString(BookingKey, JsonSerializer.Serialize(bookings));
            session.SetInt32("BookingCount", bookings.Count);
        }

        public static List<int> GetBookings(ISession session)
        {
            var json = session.GetString(BookingKey);
            return string.IsNullOrEmpty(json)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }

        public static void RemoveBooking(ISession session, int classId)
        {
            var bookings = GetBookings(session);
            bookings.Remove(classId);

            session.SetString(BookingKey, JsonSerializer.Serialize(bookings));
            session.SetInt32("BookingCount", bookings.Count);
        }
    }
}
