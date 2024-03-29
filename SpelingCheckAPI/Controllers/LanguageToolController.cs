using CommonCode.CheckResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpelingCheckAPI.Interfaces;
using SpelingCheckAPI.Services;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpPost("checkText")]
        public async Task<ActionResult<List<SpellingCheckResult>?>> CheckTextViaLT([FromBody] LTTextCheckModel model)
        {
            var result = await _languageToolService.RunGrammarCheck(model.Text, model.DisabledRules);
            return result;
        }

    }

    public class LTTextCheckModel
    {
        public string Text { get; set; } = string.Empty;
        public List<string>? DisabledRules { get; set; }
    }
}
