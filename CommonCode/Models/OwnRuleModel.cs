using CommonCode.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Models
{
    public class OwnRuleModel: IListBoxItem
    {
        public string RegexRule { get; set; } = string.Empty;
        public string Correction { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public bool CanBeDeleted { get; set; } = true;
        public string Name
        {
            get
            {
                return RegexRule;
            }
            set
            {
                Name = value;
            }
        }


        public override string ToString()
        {
            return RegexRule;
        }
    }
}
