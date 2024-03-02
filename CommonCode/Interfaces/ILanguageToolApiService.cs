using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.ReturnValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageToolParagraph = (int startIndex, CommonCode.ApiModels.LanguageToolItem ltItem);

namespace CommonCode.Interfaces
{
    public interface ILanguageToolApiService
    {
        public Task<List<SpellingCheckResult>?> CheckTextViaLanguageTool(string text);
        public Dictionary<string, LanguageToolParagraph> CreateLanguageToolItems(List<ParagraphData> paragraphs);
    }
}
