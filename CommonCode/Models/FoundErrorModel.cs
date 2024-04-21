using CommonCode.Interfaces;

namespace CommonCode.Models
{
    /// <summary>
    /// Represents a found error.
    /// </summary>
    public class FoundErrorModel : IListBoxItem
    {
        /// <summary>
        /// Gets or sets the name of the error.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the error.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the error can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of the error.
        /// </summary>
        public string ErrorType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the warning message associated with the error.
        /// </summary>
        public string WarningMessage { get; set; } = string.Empty;
    }
}
