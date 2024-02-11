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

        public async Task InsertTextToWord(string text)
        {
            await _jsRuntime.InvokeVoidAsync("formattingConnector.insertText", text);
        }

        public async Task<FormattingReturnValue> CheckParagraphs(List<string> ignoredParagraphs, string fontName, double fontSize, string alligment, double lineSpacing, double leftIndent, double rightIndent, string paraIdToCorrect = "")
        {
           return await _jsRuntime.InvokeAsync<FormattingReturnValue>("formattingConnector.checkParagraphs", ignoredParagraphs, fontName, fontSize, alligment, lineSpacing, leftIndent, rightIndent, paraIdToCorrect);
        }
    }
}
