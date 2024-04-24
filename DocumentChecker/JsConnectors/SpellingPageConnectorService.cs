using CommonCode.ReturnValues;
using Microsoft.JSInterop;
namespace DocumentChecker.JsConnectors
{
    /// <summary>
    /// Represents a service for interacting with the spelling page connector.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SpellingPageConnectorService"/> class.
    /// </remarks>
    /// <param name="jsRuntime">The JS runtime.</param>
    public class SpellingPageConnectorService(IJSRuntime jsRuntime)
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        /// <summary>
        /// Retrieves all paragraphs from the spelling connector.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="ParagraphData"/>.</returns>
        public async Task<List<ParagraphData>> GetParagrapghs()
        {
            return await _jsRuntime.InvokeAsync<List<ParagraphData>>("spellingConnector.collectAllParagraphs");
        }

        /// <summary>
        /// Selects a paragraph at the specified index.
        /// </summary>
        /// <param name="index">The index of the paragraph to select.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SelectParagraph(int index)
        {
            await _jsRuntime.InvokeVoidAsync("spellingConnector.selectParagraphAtIndex", index);
        }

        /// <summary>
        /// Replaces the selected text with the specified new text.
        /// </summary>
        /// <param name="newText">The new text to replace the selected text with.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReplaceSelectedText(string newText)
        {
            await _jsRuntime.InvokeVoidAsync("spellingConnector.replaceSelectedText", newText);
        }
    }
}
