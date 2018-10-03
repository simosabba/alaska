using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alaska.Foundation.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
            where T : struct
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string TruncateBeforeChar(this string value, char c)
        {
            int index = value.IndexOf(c);
            if (index > 0)
                value = value.Substring(0, index);
            return value;
        }

        public static string RemoveQueryString(this string value)
        {
            return value.TruncateBeforeChar('?');
        }

        public static string RemoveFirstOccurrence(this string value, string str)
        {
            return RemoveFirstOccurrence(value, str, false);
        }

        public static string RemoveFirstOccurrence(this string value, string str, bool ignoreCase)
        {
            if ((ignoreCase && value.StartsWith(str, StringComparison.InvariantCultureIgnoreCase)) ||
                (!ignoreCase && value.StartsWith(str)))
                return value.Substring(str.Length);
            return value;
        }

        public static string EnsureSlashPrefix(this string value)
        {
            return value.EnsurePrefix("/");
        }

        public static string EnsurePrefix(this string value, string prefix)
        {
            return value.StartsWith(prefix) ? value : prefix + value;
        }

        public static string EnsureSlashSuffix(this string value)
        {
            return value.EnsureSuffix("/");
        }

        public static string EnsureSuffix(this string value, string suffix)
        {
            return value.EndsWith(suffix) ? value : value + suffix;
        }

        public static string ToTitleCase(this string value)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(value.ToLower());
        }

        public static string ToKebabCase(this string value)
        {
            return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
        }

        public static string ToSnakeCase(this string value)
        {
            return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        public static string RemoveBrackets(this string value)
        {
            return value.Replace("{", string.Empty).Replace("}", string.Empty);
        }

        public static int GetCommonPrefixLength(this string value, string other, bool ignoreCase)
        {
            if (ignoreCase)
            {
                value = value.ToLower();
                other = other.ToLower();
            }

            var index = 0;
            for (; index < Math.Min(value.Length, other.Length); index++)
                if (value[index] != other[index])
                    break;

            return index;
        }
    }
}
