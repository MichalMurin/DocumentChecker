using Microsoft.AspNetCore.Components;
using DocumentChecker.JsConnectors;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using CommonCode.Models;
using static CommonCode.Deffinitions.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    /// <summary>
    /// Represents the formatting result page of the document checker.
    /// </summary>
    public partial class FormattingResultPage : BaseResultPage
    {
        [Inject]
        public FormattingPageConnectorService JsConnector { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the error.
        /// </summary>
        protected override string Errorname { get; set; } = "Chyba formátovania";

        /// <summary>
        /// Gets the formatting page data service.
        /// </summary>
        protected override FormattingPageDataService DataService
        {
            get
            {
                return DataServiceFactory.GetFormattingDataService();
            }
        }

        /// <summary>
        /// Tries to correct the paragraph.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task<bool> TryToCorrectParagraph()
        {
            if (CurrentScan is not null)
            {
                return await JsConnector.CorrectParagraph(CurrentScan.ParagraphId, DataService.FoundErrors.Select(err => ((FoundErrorModel)err).ErrorType).ToList());
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the scan result.
        /// </summary>
        /// <param name="isStart">A flag indicating whether the scan is starting.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task<ScanReturnValue> GetScanResult(bool isStart)
        {
            return await JsConnector.CheckParagraphs(isStart, DataService);
        }

        /// <summary>
        /// Sets the displayed texts based on the check state.
        /// </summary>
        /// <param name="state">The check state.</param>
        protected override void SetDisplayedTexts(CheckState state)
        {
            switch (state)
            {
                case CheckState.FOUND_ERROR:
                    Header = "Našla sa chyba!";
                    TextResult = $"Boli zistené chyby v formátovaní dokumentu. Aby bola oprava úspešná, prosím, nechajte odstavec označený";
                    break;
                case CheckState.FINISHED:
                    Header = "Kontrola dokončená!";
                    TextResult = $"Kontrola bola úspešne ukončená, nenašli sa žiadne ďalšie chyby vo formátovaní";
                    break;
                default:
                    base.SetDisplayedTexts(state);
                    break;
            }
        }

        /// <summary>
        /// Gets the error string based on the error type.
        /// </summary>
        /// <param name="errorType">The error type.</param>
        /// <returns>The error string.</returns>
        protected override string GetErrorString(string errorType)
        {
            if (FormattingErrors.ContainsKey(errorType))
            {
                return FormattingErrors[errorType];
            }
            else
            {
                return "Neznáma chyba";
            }
        }
    }
}
