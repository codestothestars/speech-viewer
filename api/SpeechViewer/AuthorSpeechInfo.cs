namespace SpeechViewer
{
    /// <summary>
    /// Basic information about an author's speech provided in response to a request for basic author or speech info.
    ///
    /// <para>
    /// Intended to be used in a context where the author is already known.
    /// Therefore the author is not included in this object.
    /// </para>
    /// </summary>
    class AuthorSpeechInfo
    {
        /// <summary>
        /// The speech's identifier in the API.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The speech's name.
        /// </summary>
        public string Name { get; set; }
    }
}
