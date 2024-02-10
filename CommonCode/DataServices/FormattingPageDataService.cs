using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.DataServices
{
    public class FormattingPageDataService
    {
        public List<string> IgnoredParagraphs { get; set; } = new List<string>();
        public double FontSize { get; set; }
        public string FontName { get; set; } = string.Empty;
        public string Alligment { get; set; } = string.Empty;
        public double LineSpacing { get; set; }
        public double LeftIndent { get; set; }
        public double RightIndent { get; set; }
    }
}
