
namespace CommonCode.ReturnValues
{
    /// <summary>
    /// Represents the return value of a scan operation.
    /// </summary>
    public class ScanReturnValue
    {
        /// <summary>
        /// Gets or sets a value indicating whether an error was found during the scan.
        /// </summary>
        public bool FoundError { get; set; }

        /// <summary>
        /// Gets or sets the ID of the paragraph where the error was found.
        /// </summary>
        public string ParagraphId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of error types found during the scan.
        /// </summary>
        public List<string> ErrorTypes { get; set; } = [];
    }
}
