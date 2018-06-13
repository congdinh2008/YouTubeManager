using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YouTubeManager.Extensions;
using YouTubeManager.Models.MediaStreams;

namespace YouTubeManager
{
    public partial class YouTubeService
    {
        /// <inheritdoc />
        public async Task<MediaStream> GetMediaStreamAsync(MediaStreamInfo info)
        {
            info.EnsureNotNull(nameof(info));

            // Get stream
            var stream = await _httpClient.GetStreamAsync(info.Url).ConfigureAwait(false);

            return new MediaStream(info, stream);
        }

        /// <inheritdoc />
        public async Task DownloadMediaStreamAsync(MediaStreamInfo info, Stream output,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            info.EnsureNotNull(nameof(info));
            output.EnsureNotNull(nameof(output));

            // Determine if stream is rate-limited
            var isRateLimited = !Regex.IsMatch(info.Url, @"ratebypass[=/]yes");

            // Download rate-limited streams in segments
            if (isRateLimited)
            {
                // Determine segment count
                const long segmentSize = 9_898_989; // this number was carefully devised through research
                var segmentCount = (int)Math.Ceiling(1.0 * info.Size / segmentSize);

                // Keep track of bytes copied for progress reporting
                var totalBytesCopied = 0L;

                for (var i = 0; i < segmentCount; i++)
                {
                    // Determine segment range
                    var from = i * segmentSize;
                    var to = (i + 1) * segmentSize - 1;

                    // Download segment
                    using (var input = await _httpClient.GetStreamAsync(info.Url, from, to).ConfigureAwait(false))
                    {
                        int bytesCopied;
                        do
                        {
                            // Copy
                            bytesCopied = await input.CopyChunkToAsync(output, cancellationToken).ConfigureAwait(false);

                            // Report progress
                            totalBytesCopied += bytesCopied;
                            progress?.Report(1.0 * totalBytesCopied / info.Size);
                        } while (bytesCopied > 0);
                    }
                }
            }
            // Download non-limited streams directly
            else
            {
                using (var input = await GetMediaStreamAsync(info).ConfigureAwait(false))
                    await input.CopyToAsync(output, progress, cancellationToken).ConfigureAwait(false);
            }
        }

#if NETSTANDARD2_0 || NET461 || NETCOREAPP2_1

        /// <inheritdoc />
        public async Task DownloadMediaStreamAsync(MediaStreamInfo info, string filePath,
            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            filePath.EnsureNotNull(nameof(filePath));

            using (var output = File.Create(filePath))
                await DownloadMediaStreamAsync(info, output, progress, cancellationToken).ConfigureAwait(false);
        }

#endif
    }
}
