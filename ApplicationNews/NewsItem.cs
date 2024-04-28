using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace ApplicationNews
{
    /// <summary>
    /// Represents a piece of news to be distributed to users
    /// </summary>
    public class NewsItem
    {
        [Key]
        public ulong Id { get; set; }
        /// <summary>
        /// The title of the news
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The body of the news update. This can be full HTML
        /// </summary>
        public string? Body { get; set; }
        /// <summary>
        /// The time this item was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The last time this item was updated
        /// </summary>
        /// <remarks>
        /// If an item that has already been seen is updated, it should marked unread
        /// </remarks>
        public DateTime UpdatedAt { get; set;}
        /// <summary>
        /// Null if not deleted, otherwise the time this item was deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// If scheduled, this is the time to stop displaying the item to users
        /// </summary>
        public DateTime? ExpiresAt { get; set; }
        /// <summary>
        /// If scheduled, this is the time to start displaying the item to users
        /// </summary>
        public DateTime? ScheduledAt { get; set; }

        /// <summary>
        /// The icon associated with this item in byte form
        /// </summary>
        public byte[]? Icon { get; set; }

        /// <summary>
        /// A link for reference for this news item
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// When true, only apps in debug mode will show this message
        /// </summary>
        public bool DevOnly { get; set; }

        /// <summary>
        /// Indicates whether this news item is published and ready to be shown to users
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Copies the settings from another <see cref="NewsItem"/>
        /// </summary>
        /// <param name="other"></param>
        public void CopyFrom(NewsItem other)
        {
            Title = other.Title;
            Body = other.Body;
            CreatedAt = other.CreatedAt;
            ScheduledAt = other.ScheduledAt;
            UpdatedAt = other.UpdatedAt;
            DeletedAt = other.DeletedAt;
            Icon = other.Icon;
            ExpiresAt = other.ExpiresAt;
            Link = other.Link;
            DevOnly = other.DevOnly;
            Published = other.Published;
        }
    }
}
