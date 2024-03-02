using CommonCode.CheckResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelingCheckAPI.Interfaces
{
    public interface IPrepositionCheckService
    {
        Task<bool> IsInstrumental(string word);

        Task<List<SpellingCheckResult>> CheckPrepositionsInText(string text);
    }
}
