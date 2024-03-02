using CommonCode.CheckResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.ApiModels
{
    /// <summary>
    /// Item that represents paragraphs that are sent to LT API in one request
    /// </summary>
    public class LanguageToolItem
    {
        public string Text { get; set; } = string.Empty;
        public int NumberOfParagraphs { get; set; } = 0;
        public List<SpellingCheckResult>? Result { get; set; }
        public Dictionary<string,int> StartIndexes { get; set; } = new Dictionary<string, int>();
    }
}
