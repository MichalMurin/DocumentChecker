using CommonCode.Extensions;

namespace CommonCode.Services.DataServices
{
    /// <summary>
    /// Represents a data service for formatting page settings.
    /// </summary>
    public class FormattingPageDataService : BaseDataService, ICloneable
    {
        /// <summary>
        /// Gets or sets the font size for heading 1.
        /// </summary>
        public double Heading1FontSize { get; set; } = 16;

        /// <summary>
        /// Gets or sets the font size for heading 2.
        /// </summary>
        public double Heading2FontSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the font size for heading 3.
        /// </summary>
        public double Heading3FontSize { get; set; } = 12;

        /// <summary>
        /// Gets or sets the font size for heading 4.
        /// </summary>
        public double Heading4FontSize { get; set; } = 11;

        /// <summary>
        /// Gets or sets the default font size.
        /// </summary>
        public double FontSize { get; set; } = 11;

        /// <summary>
        /// Gets or sets the default font name.
        /// </summary>
        public string FontName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the displayed alignment.
        /// </summary>
        public string AllingmentDispalyed { get; set; } = Deffinitions.Deffinitions.AlignmentDict.Keys.FirstOrDefault() ?? string.Empty;

        /// <summary>
        /// Gets the actual alignment based on the displayed alignment.
        /// </summary>
        public string Alligment
        {
            get
            {
                if (Deffinitions.Deffinitions.AlignmentDict.TryGetValue(AllingmentDispalyed, out string? value))
                {
                    return value;
                }
                else
                {
                    return "Justified";
                }
            }
        }

        /// <summary>
        /// Gets or sets the line spacing.
        /// </summary>
        public double LineSpacing { get; set; } = 1.5;

        /// <summary>
        /// Gets the line spacing in points.
        /// </summary>
        public double LineSpacingInPoints
        {
            get
            {
                return ((double)LineSpacing).GetLineSpacingInPoints();
            }
        }

        /// <summary>
        /// Gets or sets the left indent.
        /// </summary>
        public double LeftIndent { get; set; } = 0;

        /// <summary>
        /// Gets the left indent in points.
        /// </summary>
        public double LeftIndentInPoints
        {
            get
            {
                return LeftIndent.ConvertCmToPoints();
            }
        }

        /// <summary>
        /// Gets or sets the right indent.
        /// </summary>
        public double RightIndent { get; set; } = 0;

        /// <summary>
        /// Gets the right indent in points.
        /// </summary>
        public double RightIndentInPoints
        {
            get
            {
                return RightIndent.ConvertCmToPoints();
            }
        }

        /// <summary>
        /// Creates a new instance of the FormattingPageDataService class.
        /// </summary>
        public FormattingPageDataService()
        {
        }

        /// <summary>
        /// Creates a new instance of the FormattingPageDataService class by cloning an existing instance.
        /// </summary>
        /// <returns>A new instance of the FormattingPageDataService class.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
