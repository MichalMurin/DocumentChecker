using CommonCode.Interfaces;

namespace CommonCode.Models
{
    /// <summary>
    /// Represents a model for an own rule.
    /// </summary>
    public class OwnRuleModel : IListBoxItem
    {
        /// <summary>
        /// Gets or sets the regular expression rule.
        /// </summary>
        public string RegexRule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the correction for the rule.
        /// </summary>
        public string Correction { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message for the rule.
        /// </summary>
        public string WarningMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the rule can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; } = true;

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string Name
        {
            get
            {
                return RegexRule;
            }
            set
            {
                Name = value;
            }
        }

        /// <summary>
        /// Returns the regular expression rule as a string representation of the object.
        /// </summary>
        /// <returns>The regular expression rule.</returns>
        public override string ToString()
        {
            return RegexRule;
        }
    }
}
