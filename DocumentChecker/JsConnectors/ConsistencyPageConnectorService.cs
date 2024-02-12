using Microsoft.JSInterop;

namespace DocumentChecker.JsConnectors
{
    public class ConsistencyPageConnectorService
    {
        private readonly IJSRuntime _jsRuntime;

        public ConsistencyPageConnectorService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InsertTextToWord(string text)
        {
            await _jsRuntime.InvokeVoidAsync("consistencyConnector.insertTextTest", text);
        }

        public async Task ScanDocumentConsistency()
        {
            await _jsRuntime.InvokeVoidAsync("consistencyConnector.checkConsistency");
        }
    }
}
