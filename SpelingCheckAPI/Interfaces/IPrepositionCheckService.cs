using CommonCode.CheckResults;

namespace SpelingCheckAPI.Interfaces
{
    /// <summary>
    /// Represents a service for checking prepositions.
    /// </summary>
    public interface IPrepositionCheckService
    {
        /// <summary>
        /// Checks if a word is instrumental.
        /// </summary>
        /// <param name="word">The word to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating if the word is instrumental.</returns>
        Task<bool> IsInstrumental(string word);

        /// <summary>
        /// Checks for prepositions in a given text.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of spelling check results.</returns>
        Task<List<SpellingCheckResult>> CheckPrepositionsInText(string text);
    }
}
