using CommonCode.CheckResults;

namespace SpelingCheckAPI.Interfaces
{
    /// <summary>
    /// Represents a language tool service for grammar checking.
    /// </summary>
    public interface ILanguageToolService
    {
        /// <summary>
        /// Runs a grammar check on the specified text.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <param name="disabledRules">The list of disabled rules.</param>
        /// <returns>A list of spelling check results.</returns>
        Task<List<SpellingCheckResult>?> RunGrammarCheck(string text, List<string>? disabledRules = null);
    }
}
