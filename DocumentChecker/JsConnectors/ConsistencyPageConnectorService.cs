using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
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

        public async Task<ScanReturnValue> ScanDocumentConsistency(bool start, ConsistencyPageDataService data)
        {
            return await _jsRuntime.InvokeAsync<ScanReturnValue>("consistencyConnector.checkConsistency", start, data);
        }

        public async Task<bool> CorrectParagraph(string paraIdToCorrect, List<string> errorList)
        {
            return await _jsRuntime.InvokeAsync<bool>("consistencyConnector.corectParagraph", paraIdToCorrect, errorList);
        }
    }
}
