using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a string formed by repeating the given character given number of times.
        /// </summary>
        public static string Repeat(this char c, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return string.Empty;

            return new string(c, count);
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
    }
}
