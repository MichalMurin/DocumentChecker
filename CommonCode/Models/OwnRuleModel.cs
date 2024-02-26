using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Models
{
    public class OwnRuleModel
    {
        public string RegexRule { get; set; } = string.Empty;
        public string Correction { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public override string ToString()
        {
            return Description;
        }
    }
}
