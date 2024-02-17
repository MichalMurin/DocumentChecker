using CommonCode.DataServices;
using CommonCode.ReturnValues;
using Microsoft.JSInterop;
namespace DocumentChecker.JsConnectors
{
    public class SpellingPageConnectorService
    {
        private readonly IJSRuntime _jsRuntime;

        public SpellingPageConnectorService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<List<ParagraphData>> GetParagrapghs()
        {
            return await _jsRuntime.InvokeAsync<List<ParagraphData>>("spellingConnector.collectAllParagraphs");
        }

    }
}
