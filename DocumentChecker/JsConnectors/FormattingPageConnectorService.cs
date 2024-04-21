using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using Microsoft.JSInterop;

namespace DocumentChecker.JsConnectors
{
    /// <summary>
    /// Represents a service for connecting with the JavaScript formatting page.
    /// </summary>
    public class FormattingPageConnectorService
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormattingPageConnectorService"/> class.
        /// </summary>
        /// <param name="jsRuntime">The JS runtime.</param>
        public FormattingPageConnectorService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        /// <summary>
        /// Checks the paragraphs for formatting issues.
        /// </summary>
        /// <param name="start">A boolean value indicating whether to start the scan.</param>
        /// <param name="data">The formatting page data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the scan return value.</returns>
        public async Task<ScanReturnValue> CheckParagraphs(bool start, FormattingPageDataService data)
        {
            return await _jsRuntime.InvokeAsync<ScanReturnValue>("formattingConnector.checkFormatting", start, data);
        }

        /// <summary>
        /// Corrects the formatting of a specific paragraph.
        /// </summary>
        /// <param name="paraIdToCorrect">The ID of the paragraph to correct.</param>
        /// <param name="errors">The list of errors to correct.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the correction was successful.</returns>
        public async Task<bool> CorrectParagraph(string paraIdToCorrect, List<string> errors)
        {
            return await _jsRuntime.InvokeAsync<bool>("formattingConnector.correctFormatting", paraIdToCorrect, errors);
        }
    }
}
