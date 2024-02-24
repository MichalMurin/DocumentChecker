using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.CheckResults
{
    public class OwnRuleCheckResult
    {
        public string ErrorDescription { get; set; } = string.Empty;
        public string Correction { get; set; } = string.Empty;
        public int StartIndex { get; set; }
        public int Length { get; set; }
    }
}
