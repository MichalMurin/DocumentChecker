using CommonCode.Models;

namespace CommonCode.Services.DataServices
{
    /// <summary>
    /// Represents a base data service.
    /// </summary>
    public class BaseDataService
    {
        /// <summary>
        /// Gets or sets the ignored paragraphs.
        /// </summary>
        public virtual HashSet<string> IgnoredParagraphs { get; set; } = new HashSet<string>();

        /// <summary>
        /// Gets or sets the found errors.
        /// </summary>
        public virtual List<FoundErrorModel> FoundErrors { get; set; } = new List<FoundErrorModel>();

        /// <summary>
        /// Gets or sets a value indicating whether there is an error.
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Copies the properties from the specified source data service.
        /// </summary>
        /// <param name="source">The source data service.</param>
        public void CopyFrom(BaseDataService source)
        {
            var properties = source.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(source));
                }
            }
        }
    }
}
