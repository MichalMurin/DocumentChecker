using CommonCode.DataServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using CommonCode.Extensions;
using System.Reflection;
using DocumentChecker.JsConnectors;
using CommonCode.ReturnValues;

namespace DocumentChecker.Pages.ResultPages
{
    // TODO: Dorobit kontrolu hlaviciek a paticiek
    // TODO: Osetrit pripad ze nadpisy maju inu velkost
    // TODO: Umoznit opravit cely dokument naraz??
    // TODO: Dorobit Import a export nastaveni
    // TODO: Dropdown na vyber zarovnania
    // TODO: rednastavenen hodnoty napisat tabulatorom
    // TODO: Urobit kontrolu podla stylov, cize pre jeden styl kontrolovat vsetko .. atd??
    //      - dalo by sa to urobit tak ze si potiahnem vsetky styly z wordu a uzivatel si nastavi kazdy jeden? ... alebo budem kontrolovat len to co uz je nastavene pre tie styly
    public partial class FormattingResultPage
    {
        [Inject]
        public FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        [Inject]
        public FormattingPageConnectorService JsConnector { get; set; } = default!;
        [Parameter]
        public bool StartScan { get; set; } = false;
        public override string TextResult { get; set; } = "Kontroluje sa dokument...";
        public FormattingReturnValue? ScanResult { get; set; }

        protected async override Task OnInitializedAsync()
        {
            base.OnInitialized();
            if (StartScan)
            {
                SetHeaderAndResult();
                Console.WriteLine("Page initialized, starting scan");
                await ScanDocumentFormatting();
            }
        }
        public override async Task OnIgnoreClick()
        {
            // Add paragraph to ignored paragraphs
            // run scan again
            if (ScanResult is not null)
            {
                Console.WriteLine($"Ignoring paragraph: {ScanResult.ParagraphId}");
                FormattingPageDataService.IgnoredParagraphs.Add(ScanResult.ParagraphId);
                SetHeaderAndResult();
                await ScanDocumentFormatting();
            }


        }
        public override async Task OnCorrectClick()
        {
            // perform a correction on a paragraph and run a scan again
            if (ScanResult is not null)
            {
                Console.WriteLine($"Correcting paragraph: {ScanResult.ParagraphId}");
                SetHeaderAndResult();
                await ScanDocumentFormatting(ScanResult.ParagraphId);
            }
        }

        private async Task ScanDocumentFormatting(string paraIdToCorrect = "")
        {
            ScanResult = await JsConnector.CheckParagraphs(FormattingPageDataService.IgnoredParagraphs,
                                              FormattingPageDataService.FontName,
                                              FormattingPageDataService.FontSize,
                                              FormattingPageDataService.Alligment,
                                              FormattingPageDataService.LineSpacing.GetLineSpacingInPoints(FormattingPageDataService.FontSize),
                                              FormattingPageDataService.LeftIndent.ConvertCmToPoints(),
                                              FormattingPageDataService.RightIndent.ConvertCmToPoints(),
                                              paraIdToCorrect);
            if (ScanResult.FoundError)
            {
                Header = "Chyba!";
                TextResult = $"Boli zistené chyby v formátovaní dokumentu";
                foreach (var error in ScanResult.ErrorTypes)
                {
                    TextResult += $"\n{error}";
                }
            }
            else
            {
                Header = "Kontrola prebehla";
                TextResult = $"V dokumente sa nenašli žiadne formátovacie chyby.";
            }
            // Set Header and Result based on the result of the scan
            // TODO: check headers and footers
        }

        private void SetHeaderAndResult()
        {
            Header = "Kontrola formátovania dokumentu...";
            TextResult = "Prebieha kontrola formátovania dokumentu...";
        }
    }
}
