using System.Collections.Generic;
using JetBrains.Annotations;
using YouTubeManager.Extensions;
using YouTubeManager.Helpers;

namespace YouTubeManager.Models.ClosedCaptions
{
    /// <summary>
    /// Set of captions that get displayed during video playback.
    /// </summary>
    public class ClosedCaptionTrack
    {
        /// <summary>
        /// Metadata associated with this track.
        /// </summary>
        [NotNull]
        public ClosedCaptionTrackInfo Info { get; }

        /// <summary>
        /// Collection of closed captions that belong to this track.
        /// </summary>
        [NotNull, ItemNotNull]
        public IReadOnlyList<ClosedCaption> Captions { get; }

        /// <summary />
        public ClosedCaptionTrack(ClosedCaptionTrackInfo info, IReadOnlyList<ClosedCaption> captions)
        {
            Info = info.EnsureNotNull(nameof(info));
            Captions = captions.EnsureNotNull(nameof(captions));
        }
    }
}