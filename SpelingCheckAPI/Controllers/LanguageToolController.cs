using CommonCode.CheckResults;
using Microsoft.AspNetCore.Mvc;
using SpelingCheckAPI.Interfaces;
using SpelingCheckAPI.Services;

namespace SpelingCheckAPI.Controllers
{
    [Route("api/languageToolCheck")]
    [ApiController]
    public class LanguageToolController
    {
        private readonly ILanguageToolService _languageToolService;

        public LanguageToolController(ILanguageToolService languageToolService)
        {
            _languageToolService = languageToolService;
        }

        [HttpGet("checkText/{text}")]
        public async Task<ActionResult<List<LanguageToolCheckResult>?>> CheckTextViaLT(string text)
        {
            var result = await _languageToolService.RunGrammarCheck(text);
            return result;
        }
    }
}
