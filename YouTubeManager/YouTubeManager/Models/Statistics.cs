namespace YouTubeManager.Models
{
    /// <summary>
    /// Video statistics.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Views of this video.
        /// </summary>
        public long Views { get; }

        /// <summary>
        /// Likes of this video.
        /// </summary>
        public long Likes { get; }

        /// <summary>
        /// Dislikes of this video.
        /// </summary>
        public long Dislikes { get; }

        /// <summary />
        public Statistics(long views, long likes, long dislikes)
        {
            Views = views;
            Likes = likes;
            Dislikes = dislikes;
        }
    }
}