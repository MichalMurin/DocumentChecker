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

        public async Task SelectParagraph(int index)
        {
            await _jsRuntime.InvokeVoidAsync("spellingConnector.selectParagraphAtIndex", index);
        }

        public async Task ReplaceSelectedText(string newText)
        {
            await _jsRuntime.InvokeVoidAsync("spellingConnector.replaceSelectedText", newText);
        }   

    }
}
