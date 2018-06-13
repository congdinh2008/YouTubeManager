using System;
using System.IO;
using System.Threading.Tasks;
using YouTubeManager;
using YouTubeManager.Models.MediaStreams;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoAsync().GetAwaiter().GetResult();
        }

        private static async Task VideoAsync()
        {
            var youtubeService = new YouTubeService();
            Console.WriteLine("============== YouTube Manager ==============");
            Console.WriteLine("Enter Video ID or URL:");
            var videoId = Console.ReadLine();
            videoId = NormalizeVideoId(videoId);

            Console.WriteLine("============== Get Video Info ==============");
            var video = await youtubeService.GetVideoAsync(videoId);
            Console.WriteLine($"ID: {video.Id}\nTitle: {video.Title}\nAuthor: {video.Author}\nAuthorID: {video.AuthorId}\nUpload Date: {video.UploadDate}");

            Console.WriteLine("============== Get Video Media Stream Info ==============");
            var streamInfoSet = await youtubeService.GetVideoMediaStreamInfosAsync(videoId);
            Console.WriteLine("> " +
                              $"{streamInfoSet.Muxed.Count} muxed streams, " +
                              $"{streamInfoSet.Video.Count} video-only streams, " +
                              $"{streamInfoSet.Audio.Count} audio-only streams");

            Console.WriteLine("============== Get Highest Muxed Video Media Stream ==============");
            var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
            Console.WriteLine("Selected muxed stream with highest video quality:");
            Console.WriteLine("> " +
                              $"{streamInfo.VideoQualityLabel} video quality | " +
                              $"{streamInfo.Container} format | " +
                              $"{NormalizeFileSize(streamInfo.Size)}");

            // Compose file name, based on metadata
            var fileExtension = streamInfo.Container.GetFileExtension();
            var fileName = $"{video.Title}.{fileExtension}";

            // Replace illegal characters in file name
            fileName = fileName.Replace(Path.GetInvalidFileNameChars(), '_');

            // Download video
            Console.Write("Downloading... ");
            using (var progress = new ProgressBar())
                await youtubeService.DownloadMediaStreamAsync(streamInfo, fileName, progress);
            Console.WriteLine();

            Console.WriteLine($"Video saved to '{fileName}'");

            Console.ReadKey();
        }

        /// <summary>
        /// If given a YouTube URL, parses video id from it.
        /// Otherwise returns the same string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string NormalizeVideoId(string input)
        {
            if (!YouTubeService.TryParseVideoId(input, out var videoId))
                videoId = input;
            return videoId;
        }

        /// <summary>
        /// Turns file size in bytes into human-readable string.
        /// </summary>
        private static string NormalizeFileSize(long fileSize)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double size = fileSize;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {units[unit]}";
        }
    }
}
