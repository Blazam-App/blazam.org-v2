using System.Reflection.PortableExecutable;

namespace ApplicationNews
{
    /// <summary>
    /// Represents a piece of news to be distributed to users
    /// </summary>
    public class Settings
    {
        public double Id { get; set; }
        /// <summary>
        /// The title of the news
        /// </summary>
        public string AdminPassword { get; set; } = String.Empty;
    }
}
