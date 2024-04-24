using CommonCode.CheckResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpelingCheckAPI.Interfaces;

namespace SpelingCheckAPI.Controllers
{
    /// <summary>
    /// Controller for performing language tool checks.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LanguageToolController"/> class.
    /// </remarks>
    /// <param name="languageToolService">The language tool service.</param>
    [Route("api/languageToolCheck")]
    [ApiController]
    public class LanguageToolController(ILanguageToolService languageToolService)
    {
        private readonly ILanguageToolService _languageToolService = languageToolService;

        /// <summary>
        /// Checks the text using LanguageTool.
        /// </summary>
        /// <param name="model">The LTTextCheckModel containing the text to check.</param>
        /// <returns>A list of spelling check results.</returns>
        [HttpPost("checkText")]
        public async Task<ActionResult<List<SpellingCheckResult>?>> CheckTextViaLT([FromBody] LTTextCheckModel model)
        {
            var result = await _languageToolService.RunGrammarCheck(model.Text, model.DisabledRules);
            return result;
        }

    }

    /// <summary>
    /// Model for LanguageTool text check.
    /// </summary>
    public class LTTextCheckModel
    {
        /// <summary>
        /// Gets or sets the text to check.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the disabled rules for the check.
        /// </summary>
        public List<string>? DisabledRules { get; set; }
    }
}
