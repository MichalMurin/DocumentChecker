
namespace CommonCode.Services.DataServices
{
    /// <summary>
    /// Represents a data service for consistency page.
    /// </summary>
    public class ConsistencyPageDataService : BaseDataService
    {
        /// <summary>
        /// Gets or sets a value indicating whether the title continuity is enabled.
        /// </summary>
        public bool TitleContinutity { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the title consistency is enabled.
        /// </summary>
        public bool TitleConsistency { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether double spaces are enabled.
        /// </summary>
        public bool DoubleSpaces { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether empty lines are enabled.
        /// </summary>
        public bool EmptyLines { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether cross-reference functionality is enabled.
        /// </summary>
        public bool CrossReferenceFunctionality { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether document alignment is enabled.
        /// </summary>
        public bool DocumentAlignment { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether caption validation is enabled.
        /// </summary>
        public bool CaptionValidation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether list validation is enabled.
        /// </summary>
        public bool ListValidation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether parentheses validation is enabled.
        /// </summary>
        public bool ParenthesesValidation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether dots, commas, and colons validation is enabled.
        /// </summary>
        public bool DotsComasColonsValidation { get; set; } = true;
    }
}
