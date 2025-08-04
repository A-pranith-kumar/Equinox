using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace Equinox.Helpers
{
    public class EquinoxSession
    {
        private readonly ISession _session;
        private const string BookingKey = "BookingIds";

        // ✅ Accept HttpContext directly
        public EquinoxSession(HttpContext httpContext)
        {
            _session = httpContext.Session;
        }

        // ----- Club & Category Filters -----
        public void SetSelectedClubId(int clubId) => _session.SetInt32("SelectedClubId", clubId);
        public int GetSelectedClubId() => _session.GetInt32("SelectedClubId") ?? 0;

        public void SetSelectedCategoryId(int categoryId) => _session.SetInt32("SelectedCategoryId", categoryId);
        public int GetSelectedCategoryId() => _session.GetInt32("SelectedCategoryId") ?? 0;

        // ----- Booking Count -----
        public void SetBookingCount(int count) => _session.SetInt32("BookingCount", count);
        public int GetBookingCount() => _session.GetInt32("BookingCount") ?? 0;

        // ----- Booking ID List -----
        public void AddBookingId(int id)
        {
            var ids = GetBookingIds();
            if (!ids.Contains(id))
            {
                ids.Add(id);
                SaveBookingIds(ids);
            }
        }

        public List<int> GetBookingIds()
        {
            var json = _session.GetString(BookingKey);
            return string.IsNullOrEmpty(json)
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(json);
        }

        public void RemoveBookingId(int id)
        {
            var ids = GetBookingIds();
            if (ids.Contains(id))
            {
                ids.Remove(id);
                SaveBookingIds(ids);
            }
        }

        private void SaveBookingIds(List<int> ids)
        {
            var json = JsonSerializer.Serialize(ids);
            _session.SetString(BookingKey, json);
        }
    }
}
