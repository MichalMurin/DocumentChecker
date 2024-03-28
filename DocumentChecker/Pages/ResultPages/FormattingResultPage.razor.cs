using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CommonCode.Extensions;
using System.Reflection;
using DocumentChecker.JsConnectors;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using CommonCode.Models;
using static CommonCode.Deffinitions.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    // TODO: Dorobit kontrolu hlaviciek a paticiek
    // TODO: Umoznit opravit cely dokument naraz??
    // TODO: Dorobit Import a export nastaveni
    public partial class FormattingResultPage: BaseResultPage
    {
        [Inject]
        public FormattingPageConnectorService JsConnector { get; set; } = default!;
        protected override string Errorname { get; set; } = "Chyba formátovania";

        protected override FormattingPageDataService DataService { 
            get
            {
                return DataServiceFactory.GetFormattingDataService();
            }
        }
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

        protected override async Task<ScanReturnValue> GetScanResult(bool isStart)
        {
            return await JsConnector.CheckParagraphs(isStart, DataService);
        }

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
