using CommonCode.Extensions;
using CommonCode.Formatting;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Services.DataServices
{
    public class FormattingPageDataService : BaseDataService
    {
        public double Heading1FontSize { get; set; } = 16;
        public double Heading2FontSize { get; set; } = 14;
        public double Heading3FontSize { get; set; } = 12;
        public double Heading4FontSize { get; set; } = 11;
        public double FontSize { get; set; } = 11;
        public string FontName { get; set; } = string.Empty;
        public string AllingmentDispalyed { get; set; } = Deffinitions.AlignmentDict.Keys.FirstOrDefault() ?? string.Empty;
        public string Alligment
        {
            get
            {
                if (Deffinitions.AlignmentDict.ContainsKey(AllingmentDispalyed))
                {
                    return Deffinitions.AlignmentDict[AllingmentDispalyed];
                }
                else
                {
                    return "Justified";
                }
            }
        }
        public double LineSpacing { get; set; } = 1.5;
        public double LineSpacingInPoints
        {
            get
            {
                return ((double)LineSpacing).GetLineSpacingInPoints(FontSize);
            }
        }
        public double LeftIndent { get; set; } = 3.5;
        public double LeftIndentInPoints
        {
            get
            {
                return LeftIndent.ConvertCmToPoints();
            }
        }
        public double RightIndent { get; set; } = 2.5;
        public double RightIndentInPoints
        {
            get
            {
                return RightIndent.ConvertCmToPoints();
            }
        }
    }
}
