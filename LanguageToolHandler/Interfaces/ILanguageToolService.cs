using LanguageToolHandler.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageToolHandler.Interfaces
{
    public interface ILanguageToolService
    {
        Task<string> RunGrammarCheck(string text);
        Task<LanguageToolResult?> RunGrammarCheckViaAPi(string text);
    }
}
