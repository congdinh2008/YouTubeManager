using System.Collections.Generic;
using System.Net.Http;
using YouTubeManager.Extensions;
using YouTubeManager.Helpers;

namespace YouTubeManager
{
    /// <summary>
    /// The entry point for <see cref="YouTubeManager"/>.
    /// </summary>
    public partial class YouTubeService : IYouTubeService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, PlayerSource> _playerSourceCache;

        /// <summary>
        /// Creates an instance of <see cref="YouTubeService"/>.
        /// </summary>
        public YouTubeService(HttpClient httpClient)
        {
            _httpClient = httpClient.EnsureNotNull(nameof(httpClient));
            _playerSourceCache = new Dictionary<string, PlayerSource>();
        }

        /// <summary>
        /// Creates an instance of <see cref="YouTubeService"/>.
        /// </summary>
        public YouTubeService() : this(HttpClientExtensions.GetSingleton())
        {

        }
    }
}
