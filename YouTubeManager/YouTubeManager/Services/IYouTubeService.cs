using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using YouTubeManager.Models;
using YouTubeManager.Models.ClosedCaptions;
using YouTubeManager.Models.MediaStreams;

namespace YouTubeManager
{
    /// <summary>
    /// Interface for <see cref="YouTubeService"/>.
    /// </summary>
    public interface IYouTubeService
    {
        #region Video

        /// <summary>
        /// Gets video information by ID.
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        Task<Video> GetVideoAsync(string videoId);

        /// <summary>
        /// Gets author channel information for given video.
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        Task<Channel> GetVideoAuthorChannelAsync(string videoId);

        /// <summary>
        /// Gets a set of all available media stream infos for given video.
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        Task<MediaStreamInfoSet> GetVideoMediaStreamInfosAsync(string videoId);

        /// <summary>
        /// Gets all available closed caption track infos for given video.
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<ClosedCaptionTrackInfo>> GetVideoClosedCaptionTrackInfosAsync(string videoId);

        #endregion

        #region Playlist

        /// <summary>
        /// Gets playlist information by ID.
        /// The video list is truncated at given number of pages (1 page ≤ 200 videos).
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="maxPages"></param>
        /// <returns></returns>
        Task<Playlist> GetPlaylistAsync(string playlistId, int maxPages);

        /// <summary>
        /// Gets playlist information by ID.
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        Task<Playlist> GetPlaylistAsync(string playlistId);

        #endregion

        #region Search

        /// <summary>
        /// Searches videos using given query.
        /// The video list is truncated at given number of pages (1 page ≤ 20 videos).
        /// </summary>
        /// <param name="query"></param>
        /// <param name="maxPages"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Video>> SearchVideosAsync(string query, int maxPages);

        /// <summary>
        /// Searches videos using given query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Video>> SearchVideosAsync(string query);

        #endregion

        #region Channel

        /// <summary>
        /// Gets channel information by ID.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task<Channel> GetChannelAsync(string channelId);

        /// <summary>
        /// Gets videos uploaded by channel with given ID.
        /// The video list is truncated at given number of pages (1 page ≤ 200 videos).
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="maxPages"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Video>> GetChannelUploadsAsync(string channelId, int maxPages);

        /// <summary>
        /// Gets videos uploaded by channel with given ID.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Video>> GetChannelUploadsAsync(string channelId);

        #endregion

        #region MediaStream

        /// <summary>
        /// Gets the media stream associated with given metadata.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<MediaStream> GetMediaStreamAsync(MediaStreamInfo info);

        /// <summary>
        /// Downloads the stream associated with given metadata to the output stream.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="output"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadMediaStreamAsync(MediaStreamInfo info, Stream output,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken));

#if NETSTANDARD2_0 || NET461 || NETCOREAPP2_1

        /// <summary>
        /// Downloads the stream associated with given metadata to a file.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadMediaStreamAsync(MediaStreamInfo info, string filePath,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken));

#endif

        #endregion

        #region ClosedCaptionTrack

        /// <summary>
        /// Gets the closed caption track associated with given metadata.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<ClosedCaptionTrack> GetClosedCaptionTrackAsync(ClosedCaptionTrackInfo info);

        /// <summary>
        /// Downloads the closed caption track associated with given metadata to the output stream.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="output"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadClosedCaptionTrackAsync(ClosedCaptionTrackInfo info, Stream output,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken));

#if NETSTANDARD2_0 || NET461 || NETCOREAPP2_1

        /// <summary>
        /// Downloads the closed caption track associated with given metadata to a file.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="filePath"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DownloadClosedCaptionTrackAsync(ClosedCaptionTrackInfo info, string filePath,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken));

#endif

        #endregion
    }
}