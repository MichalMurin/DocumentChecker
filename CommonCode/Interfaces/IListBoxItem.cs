
namespace CommonCode.Interfaces
{
    /// <summary>
    /// Represents an item in a list box.
    /// </summary>
    public interface IListBoxItem
    {
        /// <summary>
        /// Gets or sets the name of the list box item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the list box item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the warning message associated with the list box item.
        /// </summary>
        public string WarningMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list box item can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; }
    }
}
