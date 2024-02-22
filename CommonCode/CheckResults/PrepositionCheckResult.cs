using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Results
{
    public class PrepositionCheckResult
    {
        public string Error { get; set; } = string.Empty;
        public string Suggestion { get; set; } = string.Empty;
        public int IndexOfPreposition { get; set; }
    }
}
