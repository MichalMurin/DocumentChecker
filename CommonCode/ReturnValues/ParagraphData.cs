using System.Text.Json.Serialization;

namespace CommonCode.ReturnValues
{
    /// <summary>
    /// Represents the data of a paragraph.
    /// </summary>
    public class ParagraphData
    {
        /// <summary>
        /// Gets or sets the index of the paragraph.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the ID of the paragraph.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text of the paragraph.
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
