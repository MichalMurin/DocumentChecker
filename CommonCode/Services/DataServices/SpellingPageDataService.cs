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
        public bool CheckPrepositions { get; set; } = true;
        public bool CheckLanguageTool { get; set; } = true;
        public bool CheckOwnRules { get; set; } = true;
    }
}
