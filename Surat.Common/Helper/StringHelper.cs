using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Helper
{
    /// <summary>
    ///   This static class contains some helper functions that operate on <see cref="String"/> objects.</summary>
    public static partial class StringHelper
    {
        /// <summary>
        ///   <p>Returns <c>null</c>, if <see cref="String"/> is <c>null</c> or empty (zero length),
        ///   otherwise string as is.</p></summary>
        /// <remarks>
        ///   <p>This function might be useful if an empty string is assumed to be <c>null</c>.</p>
        ///   <p>This is an extension method, so it can be called directly as <c>str.EmptyToNull()</c>.</p></remarks>
        /// <param name="str">
        ///   String (can be null).</param>
        /// <returns>
        ///   If <paramref name="str"/> is <c>null</c> or empty <c>null</c>, otherwise string itself</returns>
        public static string EmptyToNull(this string str)
        {
            if (str == null ||
                str.Length == 0)
                return null;
            else
                return str;
        }

        /// <summary>
        ///   <p>Returns true if <see cref="String"/> is <c>null</c> or empty (zero length)</p></summary>
        /// <remarks>
        ///   <p>This function might be useful if an empty string is assumed to be <c>null</c>.</p>
        ///   <p>This is an extension method, so it can be called directly as <c>str.IsEmptyOrNull()</c>.</p></remarks>
        /// <param name="str">
        ///   String.</param>
        /// <returns>
        ///   If <paramref name="str"/> is <c>null</c> or empty, <c>true</c></returns>
        public static bool IsEmptyOrNull(this string str)
        {
            return str == null || str.Length == 0;
        }

        /// <summary>
        ///   Checks if a string <see cref="String"/> is <c>null</c>, empty or just contains whitespace
        ///   characters.</summary>
        /// <remarks>
        ///   <p><b>Warning:</b> "\n" (line end), "\t" (tab) and some other are also considered as whitespace). 
        ///   To see a list see <see cref="String.Trim()" /> function.</p>
        ///   <p>This is an extension method, so it can be called directly as <c>str.IsTrimmedEmpty()</c>.</p></remarks>
        /// <param name="str">
        ///   String.</param>
        /// <returns>
        ///   If string is null, empty or contains only white space, <c>true</c></returns>
        public static bool IsTrimmedEmpty(this string str)
        {
            return TrimToNull(str) == null;
        }

        /// <summary>
        ///   <p>Removes whitespace characters in the left or right of the <see cref="String"/> string,
        ///   and if resulting string is empty or null, returns null.</p></summary>
        /// <remarks>
        ///   <p>Generally, when a user entered string is going to be saved to database, if user entered an
        ///   empty string, <c>null</c> or a string of whitespaces, it is stored as <c>null</c>, otherwise
        ///   it is expected to remove whitespace at start and end only.</p>
        ///   <p><b>Warning:</b> "\n" (line end), "\t" (tab) and some other are also considered as whitespace). 
        ///   To see a list see <see cref="String.Trim()" /> function.</p>
        ///   <p>This is an extension method, so it can be called directly as <c>str.TrimToNull()</c>.</p></remarks>
        /// <param name="str">
        ///   String to be trimmed.</param>
        /// <returns>
        ///   Trimmed string, result is null if empty.</returns>
        public static string TrimToNull(this string str)
        {
            if (str == null || str.Length == 0)
                return null;
            else
            {
                str = str.Trim();
                if (str.Length == 0)
                    return null;
                else
                    return str;
            }
        }

        /// <summary>
        ///   <p>Removes whitespace characters in the left or right of the <see cref="String"/> string,
        ///   and if resulting string is empty or null, returns empty.</p></summary>
        /// <remarks>
        ///   <p>Generally, when a user entered string is going to be saved to database, if user entered an
        ///   empty string, <c>null</c> or a string of whitespaces, it is stored as empty string, otherwise
        ///   it is expected to remove whitespace at start and end only.</p>
        ///   <p><b>Warning:</b> "\n" (line end), "\t" (tab) and some other are also considered as whitespace). 
        ///   To see a list see <see cref="String.Trim()" /> function.</p>
        ///   <p>This is an extension method, so it can be called directly as <c>str.TrimToEmpty()</c>.</p></remarks>
        /// <param name="str">
        ///   String to be trimmed.</param>
        /// <returns>
        ///   Trimmed string (result won't be null).</returns>
        public static string TrimToEmpty(this string str)
        {
            if (str == null || str.Length == 0)
                return String.Empty;
            else
                return str.Trim();
        }

        /// <summary>
        ///   Compares two strings ignoring whitespace at the left or right.</summary>
        /// <remarks>
        ///   <p><c>null</c> is considered to be empty.</p>
        ///   <p><b>Warning:</b> "\n" (line end), "\t" (tab) and some other are also considered as whitespace). 
        ///   To see a list see <see cref="String.Trim()" /> function.</p>
        ///   <p>This function can be used to compare a string entered by user to the value in the database
        ///   for equality.</p></remarks>
        /// <param name="string1">
        ///   String 1.</param>
        /// <param name="string2">
        ///   String 2.</param>
        /// <returns>
        ///   If two strings are same trimmed, true</returns>
        public static bool IsTrimmedSame(this string string1, string string2)
        {
            if ((string1 == null || string1.Length == 0) &&
                (string2 == null || string2.Length == 0))
                return true;
            else
                return TrimToNull(string1) == TrimToNull(string2);
        }

        public static bool IsSameIdentifier(this string one, string two)
        {
            return String.Compare(one, two, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsTrimmedSameIdent(this string one, string two)
        {
            return String.Compare(one.TrimToNull(), two.TrimToNull(), StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        ///   If the string's length is over a specified limit, trims its right and adds three points ("...").</summary>
        /// <remarks>
        ///   This is an extension method, so it can be called directly as <c>str.ThreeDots()</c>.</remarks> 
        /// <param name="str">
        ///   String.</param>
        /// <param name="maxLength">
        ///   Maksimum length for the resulting string. If given as 0, or <paramref name="str"/> is shorter
        ///   than this value, string returns as is. Otherwise <paramref name="str"/> 
        ///   it is trimmed to be under this limit in length including "the three dots".</param>
        /// <returns>
        ///   <paramref name="str"/> itself, or trimmed and three dotted string</returns>
        public static string ThreeDots(this string str, int maxLength)
        {
            if (str == null)
                return String.Empty;

            if (maxLength == 0 ||
                str.Length <= maxLength)
                return str;

            if (maxLength > 3)
                maxLength -= 3;
            else
                return "...";

            return str.Substring(0, maxLength) + "...";
        }

        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString().Replace(" ", "").Trim();
        }


        public static string ToSingleLine(this string str)
        {
            return str.TrimToEmpty().Replace("\r\n", " ").Replace("\n", " ").Trim();
        }

        public static string ToSingleQuoted(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return emptySingleQuote;

            StringBuilder sb = new StringBuilder();
            QuoteString(str, sb, false);
            return sb.ToString();
        }

        private const string emptySingleQuote = "''";
        private const string emptyDoubleQuote = "\"\"";

        /// <summary>
        ///   Quotes a string</summary>
        /// <param name="s">
        ///   String</param>
        /// <param name="sb">
        ///   StringBuilder</param>
        /// <param name="doubleQuote">
        ///   True to use double quotes</param>
        public static void QuoteString(string s, StringBuilder sb, bool doubleQuote)
        {
            if (String.IsNullOrEmpty(s))
            {
                if (doubleQuote)
                    sb.Append(emptyDoubleQuote);
                else
                    sb.Append(emptySingleQuote);
                return;
            }

            char c;
            int len = s.Length;

            sb.EnsureCapacity(sb.Length + (s.Length * 2));

            char quoteChar = doubleQuote ? '"' : '\'';
            sb.Append(quoteChar);

            for (int i = 0; i < len; i++)
            {
                c = s[i];

                switch (c)
                {
                    case '\r':
                        sb.Append(@"\r");
                        break;
                    case '\n':
                        sb.Append(@"\n");
                        break;
                    case '\t':
                        sb.Append(@"\t");
                        break;
                    case '\'':
                        if (!doubleQuote)
                            sb.Append(@"\'"); // IE doesn't understand \' in double quoted strings!!!
                        else
                            sb.Append(c);
                        break;
                    case '\"':
                        if (doubleQuote) // IE doesn't understand \" in single quoted strings!!!
                            sb.Append(@"\""");
                        else
                            sb.Append(c);
                        break;
                    case '\\':
                        sb.Append(@"\\");
                        break;
                    case '/':
                        sb.Append(@"\/");
                        break;
                    default:
                        if (c < ' ')
                        {
                            sb.Append(@"\u");
                            sb.Append(((int)c).ToString("X4", InvariantHelper.NumberFormat));
                        }
                        else
                            sb.Append(c);
                        break;
                }
            }

            sb.Append(quoteChar);
        }

        public static bool IsEmptyOrNull(this ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static string MergeListToSingleLine(List<string> list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (string s in list)
            {
                builder.Append(s);
                builder.Append(",");
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public static string MergeListToSingleLine(List<int> list)
        {
            StringBuilder builder = new StringBuilder();

            foreach (int s in list)
            {
                builder.Append(s);
                builder.Append(",");
            }

            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        //public static T ToEnum<T>(this string value, bool ignoreCase = true)
        //{
        //    return (T)Enum.Parse(typeof(T), value, ignoreCase);
        //}

        ///// <summary>
        ///// Try to parse string to your enum object. onSuccess return enum, onError return null.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <param name="ignoreCase"></param>
        ///// <returns></returns>
        //public static  System.Nullable<T> ToEnumOrNull<T>(this string value, bool ignoreCase = true) 
        //    where T:struct
        //{
        //    try
        //    {
        //        return  (T)Enum.Parse(typeof(T), value, ignoreCase);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Try to parse string to your enum object. onSuccess return enum, onError return 0 valued Enum.
        ///// if your enum has zero value it will return onError.
        ///// </summary>
        ///// <typeparam name="TEnum"></typeparam>
        ///// <param name="value"></param>
        ///// <param name="ignoreCase"></param>
        ///// <returns></returns>
        //public static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase = true) 
        //    where TEnum : struct
        //{
        //    TEnum tmp;
        //    if (!Enum.TryParse<TEnum>(value, ignoreCase, out tmp))
        //    {
        //        tmp = new TEnum();
        //    }
        //    return tmp;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Local char replace with english char. sample İ=I ü=ü </returns>
        public static string ToEnglishCode(this string str)
        {
            Encoding iso = Encoding.GetEncoding("Cyrillic");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }

        /// <summary>
        /// Repcale local culturel or special characteres from string. make it suitable to Database
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDbEntry(this string str)
        {
            var pattern = @"\W";
            // büyük W nonword(boçluk tire altçizgi nokta virgül alfabe olmayanlar diyelim) olmayanları temsil ediyor. küçük w ise word leri (alfabe harfleri diyelim) kapsıyor.
            // \w  için çıktı :: "() () () ()   (  )  !'#+%&/()}*\*--*/+,:~~^^^~~ "
            // \W  için çıktı :: "Hello1223334444Dateis15May2011iI123456789012345_6789012563qs"
            var output = System.Text.RegularExpressions.Regex.Replace(str, pattern, "");

            //eskiden bu vardı ama türkçe karakterleri ingilizceye çevirmiyor kaldırıyordu bu yüzden 2 fonksiyonu birleştirdim
            ////şimdi sıra Ingilizce karakterleri kaldırmakta.
            //output = System.Text.RegularExpressions.Regex.Replace(output, "[^\x00-\x80]+", "");
            //return output;

            //eskiden bu vardı ama türkçe karakterleri ingilizceye çevirmiyor kaldırıyordu bu yüzden 2 fonksiyonu birleştirdim
            return ToEnglishCode(output);
        }
    }
}