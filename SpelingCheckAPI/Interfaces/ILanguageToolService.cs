﻿using CommonCode.CheckResults;

namespace SpelingCheckAPI.Interfaces
{
    public interface ILanguageToolService
    {
        Task<List<LanguageToolCheckResult>?> RunGrammarCheck(string text);
    }
}