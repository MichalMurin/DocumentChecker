using CommonCode.Models;

namespace CommonCode.Services.DataServices
{
    /// <summary>
    /// Represents a data service for the spelling page.
    /// </summary>
    public class SpellingPageDataService : BaseDataService
    {
        /// <summary>
        /// Gets or sets the list of own rule models.
        /// </summary>
        public List<OwnRuleModel> Rules { get; set; } = new List<OwnRuleModel>();

        /// <summary>
        /// Gets or sets the list of disabled LanguageTool rules.
        /// </summary>
        /// <remarks>
        /// The "MORFOLOGIK_RULE_SK_SK" rule is disabled because it is already checked by the Word application itself.
        /// </remarks>
        public List<string> LanguageToolDisabledRules { get; set; } = new List<string>() { "MORFOLOGIK_RULE_SK_SK" };

        /// <summary>
        /// Gets or sets a value indicating whether to check prepositions.
        /// </summary>
        public bool CheckPrepositions { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to check LanguageTool.
        /// </summary>
        public bool CheckLanguageTool { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to check own rules.
        /// </summary>
        public bool CheckOwnRules { get; set; } = true;

        /// <summary>
        /// Gets or sets the priority of LanguageTool check.
        /// </summary>
        public int LanguageToolPriority { get; set; } = 3;

        /// <summary>
        /// Gets or sets the priority of preposition check.
        /// </summary>
        public int PrepositionCheckPriority { get; set; } = 2;

        /// <summary>
        /// Gets or sets the priority of own rules check.
        /// </summary>
        public int OwnRulesPriority { get; set; } = 1;
    }
}
