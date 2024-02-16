
namespace CommonCode.ReturnValues
{
    public class ScanReturnValue
    {
        public bool FoundError { get; set; }
        public string ParagraphId { get; set; } = string.Empty;
        public List<string> ErrorTypes { get; set; } = new List<string>();
    }
}
