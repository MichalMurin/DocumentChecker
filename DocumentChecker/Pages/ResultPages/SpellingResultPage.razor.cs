using CommonCode.DataServices;
using CommonCode.Interfaces;
using CommonCode.ReturnValues;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages.ResultPages
{
    public partial class SpellingResultPage
    {

        [Inject]
        public SpellingPageDataService SpellingPageDataService { get; set; } = default!;
        [Inject]
        public SpellingPageConnectorService JsConnector { get; set; } = default!;
        [Inject]
        public ISpellingApiService ApiServicve { get; set; } = default!;
        private int _currentIndex = 0;

        [Parameter]
        public bool StartScan { get; set; } = false;
        public override string TextResult { get; set; } = "Kontroluje sa dokument...";
        public ScanReturnValue? ScanResult { get; set; }
        private List<ParagraphData>? _paragraphs;

        protected async override Task OnInitializedAsync()
        {
            base.OnInitialized();
            if (StartScan)
            {
                SetHeaderAndResult();
                Console.WriteLine("Page initialized, starting spelling scan");
                _paragraphs = await JsConnector.GetParagrapghs();
                await StartCheck(0);
            }
        }

        //private async Task ScanDocumentSpelling(bool start = false)
        //{
            //ScanResult = await JsConnector.
            //if (ScanResult.FoundError)
            //{
            //    Header = "Chyba!";
            //    TextResult = $"Boli zistené chyby v pravopise dokumentu.  Aby bola oprava úspešná, prosím, nechajte odstavec označený";
            //    foreach (var error in ScanResult.ErrorTypes)
            //    {
            //        TextResult += $"\n{error}";
            //    }
            //}
            //else
            //{
            //    Header = "Kontrola prebehla";
            //    TextResult = $"V dokumente sa nenašli žiadne pravopisné chyby.";
            //}
            // Set Header and Result based on the result of the scan
            // TODO: check headers and footers
        //}

        private async Task StartCheck(int paragraphIndex)
        {
            if (_paragraphs is not null && paragraphIndex < _paragraphs.Count)
            {
                for (int i = paragraphIndex; i < _paragraphs.Count; i++)
                {
                    var prepositionsResult = await ApiServicve.CheckPrepositions(text: _paragraphs[i].Text);
                    if (prepositionsResult is not null)
                    {
                        foreach (var item in prepositionsResult)
                        {
                            Console.WriteLine($"Zla predlozka: {item.Error}");
                            Console.WriteLine($"Navrhovana oprava: {item.Suggestion}");
                        }
                    } 
                    var languageToolChesck = await ApiServicve.CheckLanguageTool(text: _paragraphs[i].Text);
                    if (languageToolChesck is not null)
                    {
                        foreach (var item in languageToolChesck)
                        {
                            Console.WriteLine($"Chyba: {item.ShortMessage}");
                            Console.WriteLine($"Navrhovana oprava: {item.Suggestion}");
                        }
                    }
                    if ((prepositionsResult is not null && prepositionsResult.Count > 0) ||
                        (languageToolChesck is not null && languageToolChesck.Count > 0))
                    {

                        Header = "Našla sa chyba";
                        TextResult = "";
                        await JsConnector.SelectParagraph(i);
                        _currentIndex = i;
                        break;
                    }
                }
            }
        }


        private void SetHeaderAndResult()
        {
            Header = "Kontrola pravopisu v dokumente...";
            TextResult = "Prebieha kontrola pravopisu, prosím neupravujte dokument počas prebiehajúcej kontroly...";
        }
    }
}
