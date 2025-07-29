using Microsoft.AspNetCore.Http;
using System;

namespace Equinox.Models
{
    public static class CookieHelper
    {
        public static void SetCookie(this HttpResponse response, string key, string value, int? expireDays = 7)
        {
            CookieOptions option = new CookieOptions();

            if (expireDays.HasValue)
                option.Expires = DateTime.Now.AddDays(expireDays.Value);
            else
                option.Expires = DateTime.Now.AddDays(7);

            response.Cookies.Append(key, value, option);
        }

        public static string? GetCookie(this HttpRequest request, string key)
        {
            request.Cookies.TryGetValue(key, out string? value);
            return value;
        }
    }
}