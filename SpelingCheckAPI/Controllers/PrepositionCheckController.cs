using CommonCode.Results;
using Microsoft.AspNetCore.Mvc;
using SpelingCheckAPI.Interfaces;
using SpelingCheckAPI.Services;

namespace SpelingCheckAPI.Controllers
{
    [Route("api/prepositionCheck")]
    [ApiController]
    public class PrepositionCheckController
    {
        private readonly IPrepositionCheckService _prepositionCheckService;

        public PrepositionCheckController(IPrepositionCheckService service)
        {
            _prepositionCheckService = service;
        }

        [HttpGet("checkText/{text}")]
        public async Task<ActionResult<List<PrepositionCheckResult>>> CheckPrepositionsInText(string text)
        {
            var result = await _prepositionCheckService.CheckPrepositionsInText(text);
            return result;
        }

    }
}
