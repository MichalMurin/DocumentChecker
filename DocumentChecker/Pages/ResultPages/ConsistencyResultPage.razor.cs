using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using static CommonCode.Deffinitions.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    public partial class ConsistencyResultPage
    {
        /// <summary>
        /// Gets or sets the JsConnector service for consistency page.
        /// </summary>
        [Inject]
        public ConsistencyPageConnectorService JsConnector { get; set; } = default!;

        /// <summary>
        /// Gets or sets the error name.
        /// </summary>
        protected override string Errorname { get; set; } = "Chyba konzistencie";

        /// <summary>
        /// Gets the data service for consistency page.
        /// </summary>
        protected override ConsistencyPageDataService DataService
        {
            get
            {
                return DataServiceFactory.GetConsistencyDataService();
            }
        }

        /// <summary>
        /// Handles the ignored paragraph.
        /// </summary>
        protected override async Task HandleIgnoredParagraph()
        {
            if (CurrentScan is not null)
            {
                await JsConnector.HandleIgnoredParagraph(CurrentScan.ParagraphId, CurrentScan.ErrorTypes);
            }
        }

        /// <summary>
        /// Tries to correct the paragraph.
        /// </summary>
        /// <returns>A boolean indicating whether the correction was successful or not.</returns>
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
        /// <param name="isStart">A boolean indicating whether the scan is starting or not.</param>
        /// <returns>The scan result.</returns>
        protected override async Task<ScanReturnValue> GetScanResult(bool isStart)
        {
            return await JsConnector.ScanDocumentConsistency(isStart, DataService);
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
                    TextResult = $"Boli zistené chyby v konzistencii dokumentu. Aby bola oprava úspešná, prosím, nechajte odsek označený. " +
                        $"Automatická oprava nahradí text v odseku, preto sa môže stať, že odstráni komentár pre pôvodný text.";
                    break;
                case CheckState.FINISHED:
                    Header = "Kontrola dokončená!";
                    TextResult = $"Kontrola bola úspešne ukončená, nenašli sa žiadne ďalšie chyby v konzistentnosti dokumentu";
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
            if (ConsistencyErrors.TryGetValue(errorType, out string? value))
            {
                return value;
            }
            else
            {
                return "Neznáma chyba";
            }
        }
    }
}
