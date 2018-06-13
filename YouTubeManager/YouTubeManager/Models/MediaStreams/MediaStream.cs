using System.IO;
using YouTubeManager.Extensions;

namespace YouTubeManager.Models.MediaStreams
{
    /// <summary>
    /// Stream that contains raw media data.
    /// </summary>
    public class MediaStream : Stream
    {
        private readonly Stream _stream;

        /// <summary>
        /// Metadata associated with this stream.
        /// </summary>
        public MediaStreamInfo Info { get; }

        /// <inheritdoc />
        public override bool CanRead => _stream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => _stream.CanSeek;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length => Info.Size;

        /// <inheritdoc />
        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        /// <summary />
        public MediaStream(MediaStreamInfo info, Stream stream)
        {
            Info = info.EnsureNotNull(nameof(info));
            _stream = stream.EnsureNotNull(nameof(stream));
        }

        /// <inheritdoc />
        public override void Flush() => _stream.Flush();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

        /// <inheritdoc />
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

        /// <inheritdoc />
        public override void SetLength(long value) => _stream.SetLength(value);

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

        /// <summary>
        /// Disposes resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                _stream.Dispose();
        }
    }
}