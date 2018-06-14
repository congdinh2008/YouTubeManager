using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YouTubeManager.Extensions
{
    /// <summary>
    /// Extension Methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Ensure Not Null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public static T EnsureNotNull<T>(this T o, string argName = null) where T : class
        {
            return o ?? throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Ensure Not Negative for TimeSpan.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public static TimeSpan EnsureNotNegative(this TimeSpan t, string argName = null)
        {
            return t >= TimeSpan.Zero
                ? t
                : throw new ArgumentOutOfRangeException(argName, t, "Cannot be negative.");
        }

        /// <summary>
        /// Ensure Not Negative for int.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public static int EnsureNotNegative(this int i, string argName = null)
        {
            return i >= 0
                ? i
                : throw new ArgumentOutOfRangeException(argName, i, "Cannot be negative.");
        }

        /// <summary>
        /// Ensure Not Negative for long.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public static long EnsureNotNegative(this long i, string argName = null)
        {
            return i >= 0
                ? i
                : throw new ArgumentOutOfRangeException(argName, i, "Cannot be negative.");
        }

        /// <summary>
        /// Ensure Positive.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="argName"></param>
        /// <returns></returns>
        public static int EnsurePositive(this int i, string argName = null)
        {
            return i > 0
                ? i
                : throw new ArgumentOutOfRangeException(argName, i, "Cannot be negative or zero.");
        }

        /// <summary>
        /// Check Null or White Space.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBlank(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Check not Null and White Space.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotBlank(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Substring Until.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sub"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string SubstringUntil(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? str : str.Substring(0, index);
        }

        /// <summary>
        /// Substring After.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sub"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static string SubstringAfter(this string str, string sub,
            StringComparison comparison = StringComparison.Ordinal)
        {
            var index = str.IndexOf(sub, comparison);
            return index < 0 ? string.Empty : str.Substring(index + sub.Length, str.Length - index - sub.Length);
        }

        /// <summary>
        /// Replace string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StripNonDigit(this string str)
        {
            return Regex.Replace(str, "\\D", "");
        }

        /// <summary>
        /// Parse Double
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ParseDouble(this string str)
        {
            const NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return double.Parse(str, styles, format);
        }

        /// <summary>
        /// Parse Double or Default.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ParseDoubleOrDefault(this string str, double defaultValue = default(double))
        {
            const NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return double.TryParse(str, styles, format, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Parse Int.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseInt(this string str)
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return int.Parse(str, styles, format);
        }

        /// <summary>
        /// Parse Int or Default.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseIntOrDefault(this string str, int defaultValue = default(int))
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return int.TryParse(str, styles, format, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Parse Long.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ParseLong(this string str)
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return long.Parse(str, styles, format);
        }

        /// <summary>
        /// Parse Long or Default.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ParseLongOrDefault(this string str, long defaultValue = default(long))
        {
            const NumberStyles styles = NumberStyles.AllowThousands;
            var format = NumberFormatInfo.InvariantInfo;

            return long.TryParse(str, styles, format, out var result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Parse DateTimeOffset.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseDateTimeOffset(this string str)
        {
            return DateTimeOffset.Parse(str, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AssumeUniversal);
        }

        /// <summary>
        /// Parse DateTimeOffset with format.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTimeOffset ParseDateTimeOffset(this string str, string format)
        {
            return DateTimeOffset.ParseExact(str, format, DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.AssumeUniversal);
        }

        /// <summary>
        /// Reverse string.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            var sb = new StringBuilder(str.Length);

            for (var i = str.Length - 1; i >= 0; i--)
                sb.Append(str[i]);

            return sb.ToString();
        }

        /// <summary>
        /// WebUtility.UrlEncode(string url).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return WebUtility.UrlEncode(url);
        }

        /// <summary>
        /// WebUtility.UrlDecode(string url).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return WebUtility.UrlDecode(url);
        }

        /// <summary>
        /// WebUtility.HtmlEncode(string url).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string url)
        {
            return WebUtility.HtmlEncode(url);
        }

        /// <summary>
        /// WebUtility.HtmlDecode(string url).
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string url)
        {
            return WebUtility.HtmlDecode(url);
        }

        /// <summary>
        /// Join IEnumerable T to string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        /// <summary>
        /// Split string to string[].
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separators"></param>
        /// <returns></returns>
        public static string[] Split(this string input, params string[] separators)
        {
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// EmptyIfNull.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Distinct.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> enumerable,
            Func<TSource, TKey> selector)
        {
            var existing = new HashSet<TKey>();

            foreach (var element in enumerable)
            {
                if (existing.Add(selector(element)))
                    yield return element;
            }
        }

        /// <summary>
        /// GetorDefault.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dic, TKey key,
            TValue defaultValue = default(TValue))
        {
            return dic.TryGetValue(key, out var result) ? result : defaultValue;
        }

        /// <summary>
        /// Strip Namespaces.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static XElement StripNamespaces(this XElement element)
        {
            // Original code credit: http://stackoverflow.com/a/1147012

            var result = new XElement(element);
            foreach (var e in result.DescendantsAndSelf())
            {
                e.Name = XNamespace.None.GetName(e.Name.LocalName);
                var attributes = e.Attributes()
                    .Where(a => !a.IsNamespaceDeclaration)
                    .Where(a => a.Name.Namespace != XNamespace.Xml && a.Name.Namespace != XNamespace.Xmlns)
                    .Select(a => new XAttribute(XNamespace.None.GetName(a.Name.LocalName), a.Value));
                e.ReplaceAttributes(attributes);
            }

            return result;
        }

        /// <summary>
        /// Text Extension.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string TextEx(this INode node)
        {
            if (node.NodeType == NodeType.Text)
                return node.TextContent;

            var sb = new StringBuilder();

            foreach (var child in node.ChildNodes)
                sb.Append(child.TextEx());

            if (node.NodeName == "BR")
                sb.AppendLine();

            return sb.ToString();
        }

        /// <summary>
        /// CopyChunkToAsync.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static async Task<int> CopyChunkToAsync(this Stream source, Stream destination,
            CancellationToken cancellationToken, int bufferSize = 81920)
        {
            var buffer = new byte[bufferSize];

            // Read
            var bytesCopied = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

            // Write
            await destination.WriteAsync(buffer, 0, bytesCopied, cancellationToken).ConfigureAwait(false);

            return bytesCopied;
        }

        /// <summary>
        /// CopyToAsync.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task CopyToAsync(this Stream source, Stream destination,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalBytesCopied = 0L;
            int bytesCopied;
            do
            {
                // Copy
                bytesCopied = await source.CopyChunkToAsync(destination, cancellationToken).ConfigureAwait(false);

                // Report progress
                totalBytesCopied += bytesCopied;
                progress?.Report(1.0 * totalBytesCopied / source.Length);
            } while (bytesCopied > 0);
        }
    }
}
