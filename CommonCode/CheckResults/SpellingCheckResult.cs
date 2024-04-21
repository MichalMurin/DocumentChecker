
namespace CommonCode.CheckResults
{
    /// <summary>
    /// Represents the result of a spelling check.
    /// </summary>
    public class SpellingCheckResult
    {
        /// <summary>
        /// Gets or sets the priority of the spelling check result.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the error sentence that contains the spelling error.
        /// </summary>
        public string ErrorSentence { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the suggested correction for the spelling error.
        /// </summary>
        public string Suggestion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the starting index of the spelling error in the error sentence.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the length of the spelling error in the error sentence.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the detailed message describing the spelling error.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the short message summarizing the spelling error.
        /// </summary>
        public string ShortMessage { get; set; } = string.Empty;
    }
}
