using CommonCode.CheckResults;
using Microsoft.AspNetCore.Mvc;
using SpelingCheckAPI.Interfaces;

namespace SpelingCheckAPI.Controllers
{
    /// <summary>
    /// Controller for checking prepositions in text.
    /// </summary>
    [Route("api/prepositionCheck")]
    [ApiController]
    public class PrepositionCheckController
    {
        private readonly IPrepositionCheckService _prepositionCheckService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepositionCheckController"/> class.
        /// </summary>
        /// <param name="service">The preposition check service.</param>
        public PrepositionCheckController(IPrepositionCheckService service)
        {
            _prepositionCheckService = service;
        }

        /// <summary>
        /// Checks prepositions in the given text.
        /// </summary>
        /// <param name="model">The preposition text check model.</param>
        /// <returns>A list of spelling check results.</returns>
        [HttpPost("checkText")]
        public async Task<ActionResult<List<SpellingCheckResult>>> CheckPrepositionsInText([FromBody] PrepositionTextCheckModel model)
        {
            var result = await _prepositionCheckService.CheckPrepositionsInText(model.Text);
            return result;
        }
    }
    /// <summary>
    /// Model for checking prepositions in text.
    /// </summary>
    public class PrepositionTextCheckModel
    {
        /// <summary>
        /// Gets or sets the text to check.
        /// </summary>
        public string Text { get; set; } = string.Empty;
    }
}
