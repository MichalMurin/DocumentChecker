using CommonCode.CheckResults;

namespace CommonCode.Models
{
    /// <summary>
    /// Represents a model for a found spelling error.
    /// </summary>
    public class FoundSpellingErrorModel : FoundErrorModel
    {
        /// <summary>
        /// Gets or sets the spelling check result.
        /// </summary>
        public required SpellingCheckResult SpellingCheckResult { get; set; }
    }
}
