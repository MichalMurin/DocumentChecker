using CommonCode.Extensions;
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
        public double FontSize { get; set; } = 11;
        public string FontName { get; set; } = string.Empty;
        public string Alligment { get; set; } = string.Empty;
        public double LineSpacing { get; set; } = 1.5;
        public double LineSpacingInPoints
        {
            get
            {
                return LineSpacing.GetLineSpacingInPoints(FontSize);
            }
        }
        public double LeftIndent { get; set; }
        public double LeftIndentInPoints
        {
            get
            {
                return LeftIndent.ConvertCmToPoints();
            }
        }
        public double RightIndent { get; set; }
        public double RightIndentInPoints
        {
            get
            {
                return RightIndent.ConvertCmToPoints();
            }
        }
    }
}
