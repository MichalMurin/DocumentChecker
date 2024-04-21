using CommonCode.CheckResults;
using CommonCode.ReturnValues;
using LanguageToolParagraph = (int startIndex, CommonCode.ApiModels.LanguageToolItem ltItem);

namespace CommonCode.Interfaces
{
    /// <summary>
    /// Represents the interface for the LanguageTool API service.
    /// </summary>
    public interface ILanguageToolApiService
    {
        /// <summary>
        /// Checks the given text using LanguageTool for spelling errors.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <returns>A list of spelling check results, or null if no errors were found.</returns>
        public Task<List<SpellingCheckResult>?> CheckTextViaLanguageTool(string text);

        /// <summary>
        /// Creates LanguageTool items for the given paragraphs.
        /// </summary>
        /// <param name="paragraphs">The list of paragraphs.</param>
        /// <returns>A dictionary containing the LanguageTool items for each paragraph.</returns>
        public Dictionary<string, LanguageToolParagraph> CreateLanguageToolItems(List<ParagraphData> paragraphs);
    }
}
