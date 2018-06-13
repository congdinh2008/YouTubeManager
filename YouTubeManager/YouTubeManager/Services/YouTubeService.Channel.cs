using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouTubeManager.Exceptions;
using YouTubeManager.Extensions;
using YouTubeManager.Models;

namespace YouTubeManager
{
    public partial class YouTubeService
    {
        /// <inheritdoc />
        public async Task<Channel> GetChannelAsync(string channelId)
        {
            channelId.EnsureNotNull(nameof(channelId));

            if (!ValidateChannelId(channelId))
                throw new ArgumentException($"Invalid YouTube channel ID [{channelId}].", nameof(channelId));

            // This is a hack, it gets uploads and then gets uploader info of first video

            // Get channel uploads
            var uploads = await GetChannelUploadsAsync(channelId, 1).ConfigureAwait(false);

            // Get first video
            var video = uploads.FirstOrDefault();
            if (video == null)
                throw new ParseException("Channel does not have any videos.");

            // Get video channel
            return await GetVideoAuthorChannelAsync(video.Id).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<Video>> GetChannelUploadsAsync(string channelId, int maxPages)
        {
            channelId.EnsureNotNull(nameof(channelId));
            maxPages.EnsurePositive(nameof(maxPages));

            if (!ValidateChannelId(channelId))
                throw new ArgumentException($"Invalid YouTube channel ID [{channelId}].", nameof(channelId));

            // Compose a playlist ID
            var playlistId = "UU" + channelId.SubstringAfter("UC");

            // Get playlist
            var playlist = await GetPlaylistAsync(playlistId, maxPages).ConfigureAwait(false);

            return playlist.Videos;
        }

        /// <inheritdoc />
        public Task<IReadOnlyList<Video>> GetChannelUploadsAsync(string channelId)
            => GetChannelUploadsAsync(channelId, int.MaxValue);
    }
}
