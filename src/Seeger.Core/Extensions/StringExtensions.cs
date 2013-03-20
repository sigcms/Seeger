using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Seeger
{
    public static class StringExtensions
    {
        public static string Shorten(this string str, int maxCharCount, string more)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }

            if (str.Length <= maxCharCount)
            {
                return str;
            }

            return str.Substring(0, maxCharCount) + more;
        }

        public static string[] SplitWithoutEmptyEntries(this string str, params char[] separator)
        {
            Require.NotNull(str, "str");

            if (str.Length == 0)
            {
                return new string[] { };
            }

            return str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Quote(this string str, string quote)
        {
            return Quote(str, quote, quote);
        }

        public static string Quote(this string str, string beginQuote, string endQuote)
        {
            str = str ?? String.Empty;
            beginQuote = beginQuote ?? String.Empty;
            endQuote = endQuote ?? String.Empty;

            return String.Concat(beginQuote, str, endQuote);
        }

        public static string WrapWithTag(this string str, string tag)
        {
            if (String.IsNullOrEmpty(tag))
            {
                return str;
            }

            return "<" + tag + ">" + str + "</" + tag + ">";
        }

        public static bool IgnoreCaseEquals(this string str1, string str2)
        {
            Require.NotNull(str1, "str1");

            return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IgnoreCaseStartsWith(this string str1, string str2)
        {
            Require.NotNull(str1, "str1");

            return str1.StartsWith(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IgnoreCaseEndsWith(this string str1, string str2)
        {
            Require.NotNull(str1, "str1");

            return str1.EndsWith(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static T ConvertTo<T>(this string str, TypeConverter converter = null)
        {
            return (T)ConvertTo(str, typeof(T), converter);
        }

        public static object ConvertTo(this string str, Type destinationType, TypeConverter converter = null)
        {
            Require.NotNull(str, "str");
            Require.NotNull(destinationType, "destinationType");

            if (converter == null)
            {
                converter = TypeDescriptor.GetConverter(destinationType);
            }

            return converter.ConvertFrom(str);
        }

        public static string FormatUrl(this string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return String.Empty;
            }

            return url.Replace(" ", "-").ToLower();
        }
    }
}
