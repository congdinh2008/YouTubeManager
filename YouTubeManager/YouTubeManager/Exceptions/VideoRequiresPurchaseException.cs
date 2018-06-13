﻿using System;
using YouTubeManager.Extensions;

namespace YouTubeManager.Exceptions
{
    /// <summary>
    /// Thrown when the video requires purchase and cannot be processed.
    /// </summary>
    public class VideoRequiresPurchaseException : Exception
    {
        /// <summary>
        /// ID of the video.
        /// </summary>
        public string VideoId { get; }

        /// <summary>
        /// ID of a preview video that can be watched for free.
        /// </summary>
        public string PreviewVideoId { get; }

        /// <summary />
        public VideoRequiresPurchaseException(string videoId, string previewVideoId)
            : base($"Video [{videoId}] requires purchase and cannot be processed.")
        {
            VideoId = videoId.EnsureNotNull(nameof(videoId));
            PreviewVideoId = previewVideoId.EnsureNotNull(nameof(previewVideoId));
        }
    }
}