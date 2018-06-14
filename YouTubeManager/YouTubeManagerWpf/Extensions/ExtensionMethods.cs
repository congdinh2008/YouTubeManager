using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouTubeManagerWpf.Extensions
{
    public static class ExtensionMethods
    {
        public static bool IsBlank(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotBlank(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string Reverse(this string str)
        {
            var sb = new StringBuilder(str.Length);

            for (var i = str.Length - 1; i >= 0; i--)
                sb.Append(str[i]);

            return sb.ToString();
        }

        public static string Replace(this string str, IEnumerable<char> oldChars, char newChar)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (oldChars == null)
                throw new ArgumentNullException(nameof(oldChars));

            var charArray = oldChars as char[] ?? oldChars.ToArray();
            var pos = str.IndexOfAny(charArray);
            while (pos >= 0)
            {
                str = str.Remove(pos, 1);
                str = str.Insert(pos, newChar.ToString());
                pos = str.IndexOfAny(charArray);
            }

            return str;
        }

        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static string[] Split(this string input, params string[] separators)
        {
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }
    }
}
