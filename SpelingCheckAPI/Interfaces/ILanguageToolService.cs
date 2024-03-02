using CommonCode.CheckResults;

namespace SpelingCheckAPI.Interfaces
{
    public interface ILanguageToolService
    {
        Task<List<SpellingCheckResult>?> RunGrammarCheck(string text, List<string>? disabledRules = null);
    }
}
