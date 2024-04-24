using CommonCode.CheckResults;

namespace CommonCode.ApiModels
{
    /// <summary>
    /// Item that represents paragraphs that are sent to LT API in one request
    /// </summary>
    public class LanguageToolItem
    {
        /// <summary>
        /// Gets or sets the text of the paragraphs.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of paragraphs.
        /// </summary>
        public int NumberOfParagraphs { get; set; } = 0;

        /// <summary>
        /// Gets or sets the list of spelling check results.
        /// </summary>
        public List<SpellingCheckResult>? Result { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of start indexes for each paragraph. key = paragraph ID, value = start index in text
        /// </summary>
        public Dictionary<string, int> StartIndexes { get; set; } = [];
    }
}
