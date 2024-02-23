using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Results;
using CommonCode.ReturnValues;

namespace CommonCode.Interfaces
{
    public interface ISpellingApiService
    {
        public Task<List<PrepositionCheckResult>?> CheckPrepositions(string text);
        public Task<List<LanguageToolCheckResult>?> CheckCmdLanguageTool(string text);
        public LanguageToolItem CreateLanguageToolItem(List<ParagraphData> paragraphs);
    }
}
