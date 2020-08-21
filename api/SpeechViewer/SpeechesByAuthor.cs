namespace SpeechViewer
{
    /// <summary>
    /// Basic information about an author and speeches that they wrote.
    /// </summary>
    class SpeechesByAuthor
    {
        /// <summary>
        /// The author of the speeches.
        /// </summary>
        public AuthorInfo Author { get; set; }

        /// <summary>
        /// Speeches written by the author.
        /// </summary>
        public AuthorSpeechInfo[] Speeches { get; set; }
    }
}
