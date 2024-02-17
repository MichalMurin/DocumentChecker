using CommonCode.DataServices;
using CommonCode.ReturnValues;
using Microsoft.JSInterop;

namespace DocumentChecker.JsConnectors
{
    public class FormattingPageConnectorService
    {
        private readonly IJSRuntime _jsRuntime;

        public FormattingPageConnectorService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<ScanReturnValue> CheckParagraphs(bool start, FormattingPageDataService data)
        {
           return await _jsRuntime.InvokeAsync<ScanReturnValue>("formattingConnector.checkFormatting", start, data);
        }

        public async Task<bool> CorrectParagraph(string paraIdToCorrect)
        {
            return await _jsRuntime.InvokeAsync<bool>("formattingConnector.correctFormatting", paraIdToCorrect);
        }
    }
}
