using JetBrains.Annotations;
using YouTubeManager.Extensions;

namespace YouTubeManager.Models.ClosedCaptions
{
    /// <summary>
    /// Language information.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// ISO 639-1 code of this language.
        /// </summary>
        [NotNull]
        public string Code { get; }

        /// <summary>
        /// Full English name of this language.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <summary />
        public Language(string code, string name)
        {
            Code = code.EnsureNotNull(nameof(code));
            Name = name.EnsureNotNull(nameof(name));
        }

        /// <inheritdoc />
        public override string ToString() => $"{Code} ({Name})";
    }
}