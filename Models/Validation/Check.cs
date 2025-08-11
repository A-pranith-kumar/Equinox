using System.Linq;
using Equinox.Models.DomainModels;

namespace Equinox.Models.Validation
{
    public static class Check
    {
        // for messages / inputs only (NOT used inside EF queries)
        private static string S(string? v) => (v ?? string.Empty).Trim();

        // ---------- USER ----------
        public static string? UserNameExists(EquinoxContext db, string? name, int excludeUserId = 0)
        {
            var n = S(name).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(n)) return null;

            // EF-translatable: ToLower() is translated by EF
            var exists = db.Users.Any(u =>
                u.UserId != excludeUserId &&
                u.Name != null &&
                u.Name.ToLower() == n);

            return exists ? $"Name '{S(name)}' is already in use." : null;
        }

        public static string? EmailExists(EquinoxContext db, string? email, int excludeUserId = 0)
        {
            var e = S(email).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(e)) return null;

            var exists = db.Users.Any(u =>
                u.UserId != excludeUserId &&
                u.Email != null &&
                u.Email.ToLower() == e);

            return exists ? $"Email '{S(email)}' is already in use." : null;
        }

        public static string? PhoneExists(EquinoxContext db, string? phoneNumber, int excludeUserId = 0)
        {
            // normalize input to digits only
            var p = new string(S(phoneNumber).Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(p)) return null;

            // EF-translatable: chain Replace(...) to strip common formatting from column
            var exists = db.Users.Any(u =>
                u.UserId != excludeUserId &&
                u.PhoneNumber != null &&
                u.PhoneNumber
                    .Replace("-", "")
                    .Replace(" ", "")
                    .Replace("(", "")
                    .Replace(")", "")
                    == p);

            return exists ? $"Phone number {S(phoneNumber)} is already in use." : null;
        }

        // ---------- CLUB ----------
        public static string? ClubNameExists(EquinoxContext db, string? name, int excludeClubId = 0)
        {
            var n = S(name).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(n)) return null;

            var exists = db.Clubs.Any(c =>
                c.ClubId != excludeClubId &&
                c.Name != null &&
                c.Name.ToLower() == n);

            return exists ? $"Club '{S(name)}' already exists." : null;
        }

        // ---------- CLASS CATEGORY ----------
        public static string? CategoryNameExists(EquinoxContext db, string? name, int excludeCategoryId = 0)
        {
            var n = S(name).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(n)) return null;

            var exists = db.ClassCategories.Any(cc =>
                cc.ClassCategoryId != excludeCategoryId &&
                cc.Name != null &&
                cc.Name.ToLower() == n);

            return exists ? $"Category '{S(name)}' already exists." : null;
        }
    }
}
