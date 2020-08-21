namespace SpeechViewer
{
    /// <summary>
    /// Basic information about an author provided in response to a request for basic author or speech info.
    /// </summary>
    class AuthorInfo
    {
        /// <summary>
        /// The author's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The author's identifier in the API.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The author's last name.
        /// </summary>
        public string LastName { get; set; }
    }
}
