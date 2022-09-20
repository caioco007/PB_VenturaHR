using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DTO.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToAmericanDateFormat(this DateTime me) => me.ToString("yyyy/MM/dd/", CultureInfo.InvariantCulture);

        public static string ToAmericanDateFormat(this DateTime? me)
        {
            if (me.HasValue)
                return me.Value.ToString("yyyy/MM/dd/", CultureInfo.InvariantCulture);
            else
                return null;
        }

        public static string ToBrazilianDateFormat(this DateTime me) => me.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        public static string ToBrazilianDateTimeFormat(this DateTime me) => me.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        public static string ToBrazilianDateMonthFormat(this DateTime me) => me.ToString("MM/yyyy", CultureInfo.InvariantCulture);
        public static string ToBrazilianDateTimeNoSecondsFormat(this DateTime me) => me.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

        public static string ToBrazilianTime1Format(this DateTime me) => me.ToString("HH:mm", CultureInfo.InvariantCulture);

        public static string ToBrazilianDateFormat(this DateTime? me)
        {
            if (me.HasValue)
                return me.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            else
                return null;
        }

        public static string ToBrazilianDateTimeFormat(this DateTime? me)
        {
            if (me.HasValue)
                return me.Value.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            else
                return null;
        }

        public static string ToBrazilianDateTimeNoSecondsFormat(this DateTime? me)
        {
            if (me.HasValue)
                return me.Value.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            else
                return null;
        }

        public static string ToISOString(this DateTime? me) => me?.ToString("o", CultureInfo.InvariantCulture);
        public static string ToISOString(this DateTime me) => me.ToString("o", CultureInfo.InvariantCulture);

        public static DateTime FromBrazilianDateFormat(this string me)
        {
            DateTime outValue = DateTime.MinValue;
            DateTime.TryParseExact(me, new string[] { "dd/MM/yyyy", "dd/MM/yy", "MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out outValue);
            return outValue;
        }

        public static DateTime FromBrazilianDateTimeFormat(this string me)
        {
            DateTime outValue = DateTime.MinValue;
            DateTime.TryParseExact(me, new string[] { "dd/MM/yyyy HH:mm:ss", "dd/MM/yy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out outValue);
            return outValue;
        }

        public static DateTime? FromBrazilianDateFormatNullable(this string me)
        {
            if (string.IsNullOrWhiteSpace(me))
                return null;

            DateTime outValue;
            if (DateTime.TryParseExact(me, new string[] { "dd/MM/yyyy", "dd/MM/yy", "MM/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out outValue))
                return (DateTime?)outValue;
            else
                return null;
        }

        public static DateTime? FromBrazilianDateTimeFormatNullable(this string me)
        {
            if (string.IsNullOrWhiteSpace(me))
                return null;

            DateTime outValue;
            if (DateTime.TryParseExact(me, new string[] { "dd/MM/yyyy HH:mm:ss", "dd/MM/yy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out outValue))
                return (DateTime?)outValue;
            else
                return null;
        }

        public static string ToBrazilianTimeFormat(this DateTime? me) => me?.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

        public static string ToBrazilianTimeNoSecondsFormat(this DateTime me) => me.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        public static string ToBrazilianTimeNoSecondsFormat(this DateTime? me) => me?.ToString("hh:mm tt", CultureInfo.InvariantCulture);

        public static string ToBrazilianTimeAMPMFormat(this TimeSpan? me) => me.HasValue ? DateTime.Today.Add(me.Value).ToString("hh:mm:ss tt", CultureInfo.InvariantCulture) : null;

        public static string ToBrazilianTimeNoSecondsAMPMFormat(this TimeSpan? me) => me.HasValue ? DateTime.Today.Add(me.Value).ToString("hh:mm tt", CultureInfo.InvariantCulture) : null;

        public static string ToBrazilianTimeFormat(this TimeSpan? me) => me.HasValue ? DateTime.Today.Add(me.Value).ToString("HH:mm:ss", CultureInfo.InvariantCulture) : null;

        public static string ToBrazilianTimeNoSecondsFormat(this TimeSpan? me) => me.HasValue ? DateTime.Today.Add(me.Value).ToString("HH:mm", CultureInfo.InvariantCulture) : null;

        public static TimeSpan? FromBrazilianTimeFormat(this string me)
        {
            if (DateTime.TryParseExact(me, new string[] { "hh:mm:ss tt", "h:mm:ss tt", "hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outValue)) return outValue.TimeOfDay;

            return null;
        }

        public static TimeSpan? FromBrazilianTimeNoSecondsFormat(this string me)
        {
            if (DateTime.TryParseExact(me, new string[] { "hh:mm tt", "h:mm tt", "hh:mm" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outValue)) return outValue.TimeOfDay;

            return null;
        }
    }
}
