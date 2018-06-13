using System;
using System.Runtime.Serialization;

namespace YouTubeManager.Exceptions
{
    [Serializable]
    internal class VideoIsNotAvailable : Exception
    {
        private string _videoId;
        private int _errorCode;
        private string _errorReason;

        public VideoIsNotAvailable()
        {
        }

        public VideoIsNotAvailable(string message) : base(message)
        {
        }

        public VideoIsNotAvailable(string message, Exception innerException) : base(message, innerException)
        {
        }

        public VideoIsNotAvailable(string videoId, int errorCode,
            string errorReason) : base($"Video {videoId} is not available." + $"Code: {errorCode} - Reason: {errorReason}")

        {
            _videoId = videoId;
            _errorCode = errorCode;
            _errorReason = errorReason;
        }

        protected VideoIsNotAvailable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}