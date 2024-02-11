using DocumentChecker.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace DocumentChecker.JsConnectors
{
    public class CommonJsConnectorService
    {
        private readonly IJSRuntime _jsRuntime;

        public CommonJsConnectorService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveFile(string url, string filename)
        {
            await _jsRuntime.InvokeVoidAsync("saveAsFile", url, filename);
        }

        public async Task TriggerImport(string filePickerElementId = "filepicker")
        {
            await _jsRuntime.InvokeVoidAsync("TriggerImport", filePickerElementId);
        }

        public async Task InsertTextToWord(string text)
        {
            await _jsRuntime.InvokeVoidAsync("InsertText", text);
        }
    }
}
