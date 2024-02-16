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

        public async Task<FormattingReturnValue> CheckParagraphs(bool start, FormattingPageDataService data)
        {
           return await _jsRuntime.InvokeAsync<FormattingReturnValue>("formattingConnector.checkFormatting", start, data);
        }

        public async Task<FormattingReturnValue> CorrectParagraph(string paraIdToCorrect)
        {
            return await _jsRuntime.InvokeAsync<FormattingReturnValue>("formattingConnector.correctFormatting", paraIdToCorrect);
        }
    }
}
