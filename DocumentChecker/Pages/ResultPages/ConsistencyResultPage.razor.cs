using CommonCode.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;

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

        private async Task ScanDocumentFormatting(string paraIdToCorrect = "")
        {
            await JsConnector.ScanDocumentConsistency();
        }

        private void SetHeaderAndResult()
        {
            Header = "Kontrola konzistnentnosti dokumentu...";
            TextResult = "Prebieha kontrola konzistnentnosti dokumentu...";
        }
    }
}
