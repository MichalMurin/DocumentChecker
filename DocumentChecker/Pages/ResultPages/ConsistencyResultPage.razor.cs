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
    }
}
