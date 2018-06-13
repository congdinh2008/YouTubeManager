using JetBrains.Annotations;

namespace YouTubeManager.Models
{
    /// <summary>
    /// Thumbnails collection of this video.
    /// </summary>
    public class Thumbnails
    {
        private readonly string _videoId;

        /// <summary>
        /// Low resolution thumbnail URL of this video.
        /// </summary>
        [NotNull]
        public string LowResThumbnailURL
            => $"https://img.youtube.com/vi/{_videoId}/default.jpg";

        /// <summary>
        /// Medium resolution thumbnail URL of this video.
        /// </summary>
        [NotNull]
        public string MediumResThumbnailURL
            => $"https://img.youtube.com/vi/{_videoId}/mqdefault.jpg";

        /// <summary>
        /// High resolution thumbnail URL of this video.
        /// </summary>
        [NotNull]
        public string HighResThumbnailURL
            => $"https://img.youtube.com/vi/{_videoId}/hqdefault.jpg";

        /// <summary>
        /// Standard resolution thumbnail URL of this video.
        /// </summary>
        [NotNull]
        public string StandardResThumbnailURL
            => $"https://img.youtube.com/vi/{_videoId}/sddefault.jpg";

        /// <summary>
        /// Maximum resolution thumbnail URL of this video.
        /// </summary>
        [NotNull]
        public string MaxResThumbnailURL
            => $"https://img.youtube.com/vi/{_videoId}/maxresdefault.jpg";

        /// <summary />
        public Thumbnails(string videoId)
        {
            _videoId = videoId;
        }
    }
}