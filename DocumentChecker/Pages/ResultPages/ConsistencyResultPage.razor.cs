using CommonCode.Interfaces;
using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using static CommonCode.Formatting.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    public partial class ConsistencyResultPage
    {
        [Inject]
        public ConsistencyPageConnectorService JsConnector { get; set; } = default!;
        protected override ConsistencyPageDataService DataService
        {
            get
            {
                return DataServiceFactory.GetConsistencyDataService();
            }
        }

        protected override async Task HandleIgnoredParagraph()
        {
            if (CurrentScan is not null)
            {
                await JsConnector.HandleIgnoredParagraph(CurrentScan.ParagraphId, CurrentScan.ErrorTypes);
            }
        }

        protected override async Task<bool> TryToCorrectParagraph()
        {
            if (CurrentScan is not null)
            {
                return await JsConnector.CorrectParagraph(CurrentScan.ParagraphId, DataService.FoundErrors.Select(err => ((DisplayedErrorModel)err).ErrorType).ToList());
            }
            else
            {
                return false;
            }
        }

        protected override async Task<ScanReturnValue> GetScanResult(bool isStart)
        {
            return await JsConnector.ScanDocumentConsistency(isStart, DataService);
        }

        protected override void SetDisplayedTexts(CheckState state)
        {
            switch (state)
            {
                case CheckState.FOUND_ERROR:
                    Header = "Našla sa chyba!";
                    TextResult = $"Boli zistené chyby v konzistencii dokumentu. Aby bola oprava úspešná, prosím, nechajte odstavec označený";
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

        protected override string GetErrorString(string errorType)
        {
            if (ConsistencyErrors.ContainsKey(errorType))
            {
                return ConsistencyErrors[errorType];
            }
            else
            {
                return "Neznáma chyba";
            }
        }
    }
}
