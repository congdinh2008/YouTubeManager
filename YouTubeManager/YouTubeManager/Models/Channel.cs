using JetBrains.Annotations;
using YouTubeManager.Extensions;

namespace YouTubeManager.Models
{
    /// <summary>
    /// Information about a YouTube channel.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// ID of this channel.
        /// </summary>
        [NotNull]
        public string Id { get; }

        /// <summary>
        /// Title of this channel.
        /// </summary>
        [NotNull]
        public string Title { get; }

        /// <summary>
        /// Logo image URL of this channel.
        /// </summary>
        [NotNull]
        public string LogoUrl { get; }

        /// <summary />
        public Channel(string id, string title, string logoUrl)
        {
            Id = id.EnsureNotNull(nameof(id));
            Title = title.EnsureNotNull(nameof(title));
            LogoUrl = logoUrl.EnsureNotNull(nameof(logoUrl));
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }
}