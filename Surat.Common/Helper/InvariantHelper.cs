using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Surat.Common.Helper
{
    public static class InvariantHelper
    {
        /// <summary>
        ///   Number format information for invariant culture</summary>
        public static readonly NumberFormatInfo NumberFormat;

        /// <summary>
        ///   Date time format information for invariant culture</summary>
        public static readonly DateTimeFormatInfo DateTimeFormat;

        static InvariantHelper()
        {
            NumberFormat = NumberFormatInfo.InvariantInfo;
            DateTimeFormat = DateTimeFormatInfo.InvariantInfo;
        }

        public static string ToInvariant(this int value)
        {
            return value.ToString(NumberFormat);
        }

        public static string ToInvariant(this Int64 value)
        {
            return value.ToString(NumberFormat);
        }

        public static string ToInvariant(this Double value)
        {
            return value.ToString(NumberFormat);
        }

        public static string ToInvariant(this Decimal value)
        {
            return value.ToString(NumberFormat);
        }

        /// <summary>
        ///   ISO Date and Time Format (up to milliseconds).</summary>
        public static string ISODateTimeFormatUTC =
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";

        /// <summary>
        ///   ISO Date and Time Format (up to milliseconds).</summary>
        public static string ISODateTimeFormatLocal =
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff";

        private static string[] _isoDateTimeFormats = {
            "yyyy'-'MM'-'dd",
            "yyyy'-'MM'-'dd'T'HH':'mm",
            "yyyy'-'MM'-'dd'T'HH':'mmK",
            "yyyy'-'MM'-'dd'T'HH':'mm'Z'",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ssK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK",
            "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"
        };

        /// <summary>
        ///   Tries to parse an ISO 8601 date-time string.</summary>
        /// <param name="value">
        ///   String to be parsed</param>
        /// <param name="date">
        ///   Parameter to return parsed date-time value in.</param>
        /// <returns>
        ///   True if string is a valid ISO8601 date-time string.</returns>
        public static bool TryParseISO8601DateTime(this string value, out DateTime date)
        {
            return DateTime.TryParseExact(value, _isoDateTimeFormats, InvariantHelper.DateTimeFormat,
                DateTimeStyles.None, out date);
        }

    }
}
