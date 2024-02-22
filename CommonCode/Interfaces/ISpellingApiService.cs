using CommonCode.CheckResults;
using CommonCode.Results;

namespace CommonCode.Interfaces
{
    public interface ISpellingApiService
    {
        public Task<List<PrepositionCheckResult>?> CheckPrepositions(string text);
        public Task<List<LanguageToolCheckResult>?> CheckLanguageTool(string text);
    }
}
