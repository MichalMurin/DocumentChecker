using Microsoft.JSInterop;

namespace DocumentChecker.JsConnectors
{
    /// <summary>
    /// Provides common JavaScript interop functionality for the application.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CommonJsConnectorService"/> class.
    /// </remarks>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    public class CommonJsConnectorService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        /// <summary>
        /// Saves a file from the specified URL with the given filename.
        /// </summary>
        /// <param name="url">The URL of the file to save.</param>
        /// <param name="filename">The filename to save the file as.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SaveFile(string url, string filename)
        {
            await _jsRuntime.InvokeVoidAsync("saveAsFile", url, filename);
        }

        /// <summary>
        /// Triggers the import process using the specified file picker element ID.
        /// </summary>
        /// <param name="filePickerElementId">The ID of the file picker element.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task TriggerImport(string filePickerElementId = "filepicker")
        {
            await _jsRuntime.InvokeVoidAsync("TriggerImport", filePickerElementId);
        }

        /// <summary>
        /// Inserts the specified text into the Word document.
        /// </summary>
        /// <param name="text">The text to insert.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InsertTextToWord(string text)
        {
            await _jsRuntime.InvokeVoidAsync("InsertText", text);
        }
    }
}
