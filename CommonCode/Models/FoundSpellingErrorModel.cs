using CommonCode.CheckResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode.Models
{
    public class FoundSpellingErrorModel: FoundErrorModel
    {
        public required SpellingCheckResult SpellingCheckResult { get; set; }
    }
}
