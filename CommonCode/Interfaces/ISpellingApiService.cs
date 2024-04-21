using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.ReturnValues;

namespace CommonCode.Interfaces
{
    /// <summary>
    /// Represents the interface for a spelling API service.
    /// </summary>
    public interface ISpellingApiService
    {
        /// <summary>
        /// Checks the prepositions s/z in the given text.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="priority">The priority of the check.</param>
        /// <returns>The API result containing the list of spelling check results.</returns>
        public Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositions(string text, int priority);

        /// <summary>
        /// Checks the text using the CmdLanguageTool.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="priority">The priority of the check.</param>
        /// <param name="disabledRules">The list of disabled rules.</param>
        /// <returns>The API result containing the list of spelling check results.</returns>
        public Task<APIResult<List<SpellingCheckResult>?>> CheckCmdLanguageTool(string text, int priority, List<string>? disabledRules = null);

        /// <summary>
        /// Creates a LanguageTool item from the given paragraphs.
        /// </summary>
        /// <param name="paragraphs">The list of paragraphs.</param>
        /// <returns>The LanguageTool item.</returns>
        public LanguageToolItem CreateLanguageToolItem(List<ParagraphData> paragraphs);
    }
}
