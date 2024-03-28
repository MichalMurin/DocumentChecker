using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.ReturnValues;

namespace CommonCode.Interfaces
{
    public interface ISpellingApiService
    {
        public Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositions(string text);
        public Task<APIResult<List<SpellingCheckResult>?>> CheckCmdLanguageTool(string text, List<string>? disabledRules = null);
        public LanguageToolItem CreateLanguageToolItem(List<ParagraphData> paragraphs);
    }
}
