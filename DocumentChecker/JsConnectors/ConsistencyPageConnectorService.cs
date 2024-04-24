using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using Microsoft.JSInterop;

namespace DocumentChecker.JsConnectors
{
    /// <summary>
    /// Represents a service for connecting with the JavaScript functions related to document consistency.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConsistencyPageConnectorService"/> class.
    /// </remarks>
    /// <param name="jsRuntime">The JS runtime instance.</param>
    public class ConsistencyPageConnectorService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        /// <summary>
        /// Scans the document consistency.
        /// </summary>
        /// <param name="start">A flag indicating whether to start the scan.</param>
        /// <param name="data">The consistency page data.</param>
        /// <returns>The scan result.</returns>
        public async Task<ScanReturnValue> ScanDocumentConsistency(bool start, ConsistencyPageDataService data)
        {
            return await _jsRuntime.InvokeAsync<ScanReturnValue>("consistencyConnector.checkConsistency", start, data);
        }

        /// <summary>
        /// Corrects a paragraph.
        /// </summary>
        /// <param name="paraIdToCorrect">The ID of the paragraph to correct.</param>
        /// <param name="errorList">The list of errors.</param>
        /// <returns>A flag indicating whether the paragraph was corrected successfully.</returns>
        public async Task<bool> CorrectParagraph(string paraIdToCorrect, List<string> errorList)
        {
            return await _jsRuntime.InvokeAsync<bool>("consistencyConnector.corectParagraph", paraIdToCorrect, errorList);
        }

        /// <summary>
        /// Handles an ignored paragraph.
        /// </summary>
        /// <param name="paraIdToIgnore">The ID of the paragraph to ignore.</param>
        /// <param name="errorList">The list of errors.</param>
        /// <returns>A flag indicating whether the ignored paragraph was handled successfully.</returns>
        public async Task<bool> HandleIgnoredParagraph(string paraIdToIgnore, List<string> errorList)
        {
            return await _jsRuntime.InvokeAsync<bool>("consistencyConnector.handleIgnoredParagraph", paraIdToIgnore, errorList);
        }
    }
}
