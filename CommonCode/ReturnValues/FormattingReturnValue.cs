using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.ReturnValues
{
    public class FormattingReturnValue
    {
        public bool FoundError { get; set; }
        public string ParagraphId { get; set; } = string.Empty;
        public List<string> ErrorTypes { get; set; } = new List<string>();
    }
}
