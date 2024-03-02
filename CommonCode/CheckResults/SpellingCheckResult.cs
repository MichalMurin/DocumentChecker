using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.CheckResults
{
    public class SpellingCheckResult
    {
        public string ErrorSentence { get; set; } = string.Empty;
        public string Suggestion { get; set; } = string.Empty;
        public int Index { get; set; }
        public int Length { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ShortMessage { get; set; } = string.Empty;
    }
}
