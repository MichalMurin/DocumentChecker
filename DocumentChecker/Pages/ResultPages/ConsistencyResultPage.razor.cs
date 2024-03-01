using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace DocumentChecker.Pages.ResultPages
{
    public partial class ConsistencyResultPage
    {
        [Inject]
        public ConsistencyPageDataService ConsistencyPageDataService { get; set; } = default!;
        [Inject]
        public ConsistencyPageConnectorService JsConnector { get; set; } = default!;
        [Parameter]
        public bool StartScan { get; set; } = false;
        public override string TextResult { get; set; } = "Kontroluje sa dokument...";
        public ScanReturnValue? ScanResult { get; set; }

        protected async override Task OnInitializedAsync()
        {
            base.OnInitialized();
            if (StartScan)
            {
                SetHeaderAndResult();
                Console.WriteLine("Page initialized, starting scan");
                await ScanDocumentConsistency(true);
            }
        }

        public override async Task OnCorrectClick()
        {
            // perform a correction on a paragraph and run a scan again
            if (ScanResult is not null)
            {
                Console.WriteLine($"Correcting paragraph: {ScanResult.ParagraphId}");
                SetHeaderAndResult();
                var correctionResult = await JsConnector.CorrectParagraph(ScanResult.ParagraphId, ScanResult.ErrorTypes);
                if (!correctionResult)
                {
                    HandleCorrectionResult(correctionResult);
                }
                else
                {
                    await ScanDocumentConsistency();
                }
            }
        }

        public override async Task OnIgnoreClick()
        {
            if (ScanResult is not null)
            {
                Console.WriteLine($"Ignoring paragraph: {ScanResult.ParagraphId}");
                ConsistencyPageDataService.IgnoredParagraphs.Add(ScanResult.ParagraphId);
                SetHeaderAndResult();
                await JsConnector.HandleIgnoredParagraph(ScanResult.ParagraphId, ScanResult.ErrorTypes);
                await ScanDocumentConsistency();
            }
        }

        private async Task ScanDocumentConsistency(bool start = false, bool ignore = false)
        {
            ScanResult = await JsConnector.ScanDocumentConsistency(start, ConsistencyPageDataService);
            if (ScanResult.FoundError)
            {
                Header = "Chyba!";
                TextResult = $"Boli zistené chyby v konzistnentnosti dokumentu. Aby bola oprava úspešná, prosím, nechajte odstavec označený";
                foreach (var error in ScanResult.ErrorTypes)
                {
                    TextResult += $"\n{error}";
                }
            }
            else
            {
                Header = "Kontrola prebehla";
                TextResult = $"V dokumente sa nenašli žiadne chyby konzistencie.";
            }
        }

        private void SetHeaderAndResult()
        {
            Header = "Kontrola konzistnentnosti dokumentu...";
            TextResult = "Prebieha kontrola konzistnentnosti dokumentu, prosím neupravujte dokument počas prebiehajúcej kontroly...";
        }
    }
}
