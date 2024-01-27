using System.Text.Json.Serialization;

namespace DocumentChecker.ReturnValues
{
    public class Font
    {
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; } = default!;

        [JsonPropertyName("doubleStrikeThrough")]
        public bool DoubleStrikeThrough { get; set; }

        [JsonPropertyName("highlightColor")]
        public object HighlightColor { get; set; } = default!;

        [JsonPropertyName("italic")]
        public bool Italic { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("strikeThrough")]
        public bool StrikeThrough { get; set; }

        [JsonPropertyName("subscript")]
        public bool Subscript { get; set; }

        [JsonPropertyName("superscript")]
        public bool Superscript { get; set; }

        [JsonPropertyName("underline")]
        public string Underline { get; set; } = default!;
    }

    public class Paragraph
    {
        [JsonPropertyName("alignment")]
        public string Alignment { get; set; } = default!;

        [JsonPropertyName("firstLineIndent")]
        public int FirstLineIndent { get; set; }

        [JsonPropertyName("isLastParagraph")]
        public bool IsLastParagraph { get; set; }

        [JsonPropertyName("isListItem")]
        public bool IsListItem { get; set; }

        [JsonPropertyName("leftIndent")]
        public int LeftIndent { get; set; }

        [JsonPropertyName("lineSpacing")]
        public double LineSpacing { get; set; }

        [JsonPropertyName("lineUnitAfter")]
        public double LineUnitAfter { get; set; }

        [JsonPropertyName("lineUnitBefore")]
        public double LineUnitBefore { get; set; }

        [JsonPropertyName("outlineLevel")]
        public int OutlineLevel { get; set; }

        [JsonPropertyName("rightIndent")]
        public int RightIndent { get; set; }

        [JsonPropertyName("spaceAfter")]
        public int SpaceAfter { get; set; }

        [JsonPropertyName("spaceBefore")]
        public int SpaceBefore { get; set; }

        [JsonPropertyName("style")]
        public string Style { get; set; } = default!;

        [JsonPropertyName("styleBuiltIn")]
        public string StyleBuiltIn { get; set; } = default!;

        [JsonPropertyName("tableNestingLevel")]
        public int TableNestingLevel { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;

        [JsonPropertyName("uniqueLocalId")]
        public string UniqueLocalId { get; set; } = default!;
    }

    public class WordInfo
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;

        [JsonPropertyName("font")]
        public Font Font { get; set; } = default!;

        [JsonPropertyName("style")]
        public string Style { get; set; } = default!;

        [JsonPropertyName("paragraphs")]
        public List<Paragraph> Paragraphs { get; set; } = default!;
    }
}
