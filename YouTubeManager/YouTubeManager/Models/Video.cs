using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using YouTubeManager.Extensions;

namespace YouTubeManager.Models
{
    /// <summary>
    /// Video Information
    /// </summary>
    public class Video
    {
        /// <summary>
        /// ID of this video.
        /// </summary>
        [NotNull]
        public string Id { get; }

        /// <summary>
        /// Title of this video.
        /// </summary>
        [NotNull]
        public string Title { get; }

        /// <summary>
        /// Description of this video.
        /// </summary>
        [NotNull]
        public string Description { get; }

        /// <summary>
        /// Author of this video.
        /// </summary>
        [NotNull]
        public string Author { get; }

        /// <summary>
        /// Author ID of this video.
        /// </summary>
        public string AuthorId { get; }

        /// <summary>
        /// Upload date of this video.
        /// </summary>
        public DateTimeOffset UploadDate { get; }

        /// <summary>
        /// Duration of this video.
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Keywords of this video.
        /// </summary>
        [NotNull, ItemNotNull]
        public IReadOnlyList<string> Keywords { get; }

        /// <summary>
        /// Thumbnails of this video.
        /// </summary>
        [NotNull]
        public Thumbnails Thumbnails { get; }

        /// <summary>
        /// Statistics (Like, Dislike, View) of this video.
        /// </summary>
        [NotNull]
        public Statistics Statistics { get; }

        /// <summary />
        public Video(string id, string title, string description,
            string author, string authorId, DateTimeOffset uploadDate,
            TimeSpan duration, IReadOnlyList<string> keywords,
            Thumbnails thumbnails, Statistics statistics)
        {
            Id = id.EnsureNotNull(nameof(id));
            Title = title.EnsureNotNull(nameof(title));
            Description = description.EnsureNotNull(nameof(description));
            Author = author.EnsureNotNull(nameof(author));
            AuthorId = authorId.EnsureNotNull(nameof(authorId));
            UploadDate = uploadDate;
            Duration = duration.EnsureNotNegative(nameof(duration));
            Keywords = keywords.EnsureNotNull(nameof(keywords));
            Thumbnails = thumbnails.EnsureNotNull(nameof(thumbnails));
            Statistics = statistics.EnsureNotNull(nameof(statistics));
        }

        /// <inheritdoc />
        public override string ToString() => Title;
    }
}