using CommonCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Services.DataServices
{
    public class SpellingPageDataService : BaseDataService
    {
        public List<OwnRuleModel> Rules { get; set; } = new List<OwnRuleModel>();
        // Disabling rule MORFOLOGIK_RULE_SK_SK because it is already checked by Word application itself
        public List<string> LanguageToolDisabledRules { get; set; } = new List<string>() { "MORFOLOGIK_RULE_SK_SK" };
        public bool CheckPrepositions { get; set; } = true;
        public bool CheckLanguageTool { get; set; } = true;
        public bool CheckOwnRules { get; set; } = true;
        public int LanguageToolPriority { get; set; } = 3;
        public int PrepositionCheckPriority { get; set; } = 2;
        public int OwnRulesPriority { get; set; } = 1;
    }
}
