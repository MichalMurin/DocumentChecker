using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.DataServices;
using CommonCode.Interfaces;
using CommonCode.Results;
using CommonCode.ReturnValues;
using CommonCode.Services;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Dynamic;
using SpellingResults = (System.Collections.Generic.List<CommonCode.Results.PrepositionCheckResult>? prepositionCheckResults, System.Collections.Generic.List<CommonCode.CheckResults.LanguageToolCheckResult>? languageToolResults);

namespace DocumentChecker.Pages.ResultPages
{
    public partial class SpellingResultPage
    {

        [Inject]
        public SpellingPageDataService SpellingPageDataService { get; set; } = default!;
        [Inject]
        public SpellingPageConnectorService JsConnector { get; set; } = default!;
        [Inject]
        public ISpellingApiService SpellingApiService { get; set; } = default!;

        [Parameter]
        public bool StartScan { get; set; } = false;
        public override string TextResult { get; set; } = "Kontroluje sa dokument...";
        public ScanReturnValue? ScanResult { get; set; }
        private List<ParagraphData>? _paragraphs;
        private LanguageToolItem? _ltItem;
        private int _currentIndex = 0;
        private SpellingResults _currentSpellingResult;

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

        public override async Task OnIgnoreClick()
        {
            if (_paragraphs is not null)
            {
                SpellingPageDataService.IgnoredParagraphs.Add(_paragraphs[_currentIndex].Id);
                await StartCheck(_currentIndex + 1);
            }
            else
            {
                TextResult = "Vyskytla sa neočakávaná chyba, prosím spustite kontrolu nanovo.";
            }
        }
        public override async Task OnCorrectClick()
        {
            if (_paragraphs is not null)
            {
                // replace selected wrong text with the new one
                await JsConnector.ReplaceSelectedText(CorrectParagraph(_paragraphs[_currentIndex].Text ,_currentSpellingResult));
                await StartCheck(_currentIndex + 1);
            }
            else
            {
                TextResult = "Vyskytla sa neočakávaná chyba, prosím spustite kontrolu nanovo.";
            }
        }

        private async Task StartCheck(int paragraphIndex)
        {
            if (_paragraphs is not null && paragraphIndex < _paragraphs.Count)
            {
                for (int i = paragraphIndex; i < _paragraphs.Count; i++)
                {
                    List<PrepositionCheckResult>? prepositionCheckResults = null;
                    if (SpellingPageDataService.CheckPrepositions)
                    {
                        prepositionCheckResults = await CheckPrepositionInParagraph(_paragraphs[i]);
                    }
                    List<LanguageToolCheckResult>? languageToolResults = null;
                    if (SpellingPageDataService.CheckLanguageTool)
                    {
                        languageToolResults = await CheckLanguageToolInParagraph(_paragraphs[i]);
                    }                    

                    if ((prepositionCheckResults is not null && prepositionCheckResults.Count > 0) ||
                        (languageToolResults is not null && languageToolResults.Count > 0))
                    {
                        Header = "Našla sa chyba";
                        _currentSpellingResult = (prepositionCheckResults, languageToolResults);
                        TextResult = CollectErrorsInString(_currentSpellingResult);
                        await JsConnector.SelectParagraph(i);
                        _currentIndex = i;
                        break;
                    }
                }
            }
            SetFinishResults();
        }

        private async Task<List<LanguageToolCheckResult>?> CheckLanguageToolInParagraph(ParagraphData paragraph)
        {
            if (_ltItem is null)
            {
                _ltItem = SpellingApiService.CreateLanguageToolItem(_paragraphs!);
            }
            var startIndex = _ltItem.StartIndexes[paragraph.Id];
            var languageToolResults = _ltItem.Result;
            if (languageToolResults is null)
            {
                _ltItem.Result = await SpellingApiService.CheckCmdLanguageTool(_ltItem.Text);
                languageToolResults = _ltItem.Result;
            }
            var relevantCheckResults = languageToolResults?.Where(x => x.Index >= startIndex && x.Index <= startIndex + paragraph.Text.Length).ToList();
            if (relevantCheckResults is not null)
            {
                foreach (var item in relevantCheckResults)
                {
                    Console.WriteLine($"Chyba: {item.ShortMessage}");
                    Console.WriteLine($"Navrhovana oprava: {item.Suggestion}");
                    Console.WriteLine($"Navrhovany index: {item.Index}");
                    Console.WriteLine($"Index vzhladom na paragraf: {item.Index - startIndex}");
                }
            }
            return relevantCheckResults;
        }

        private async Task<List<PrepositionCheckResult>?> CheckPrepositionInParagraph(ParagraphData paragraph)
        {
            var prepositionsResult = await SpellingApiService.CheckPrepositions(text: paragraph.Text);
            if (prepositionsResult is not null)
            {
                foreach (var item in prepositionsResult)
                {
                    Console.WriteLine($"Zla predlozka: {item.Error}");
                    Console.WriteLine($"Navrhovana oprava: {item.Suggestion}");
                }
            }
            return prepositionsResult;
        }

        private string CollectErrorsInString(SpellingResults results)
        {
            string result = string.Empty;
            if (results.languageToolResults is not null)
            {
                foreach (var err in results.languageToolResults)
                {
                    result += $"Vo vete \"{err.ErrorSentence}\" sa našla chyba:\n \"{err.ShortMessage}\"\n";
                }
            }
            if (results.prepositionCheckResults is not null)
            {
                foreach (var err in results.prepositionCheckResults)
                {
                    result += $"Našla sa chyba v predložke \"{err.Error}\"\n";
                }
            }
            return result;
        }

        private string CorrectParagraph(string paragraph, SpellingResults results)
        {// Generated - need to check
            //TODO : pouzi iny index .. start index
            // TODO pri predlozkach tiez pouzi index aby sa nestalo ze nahradime zle slovo
            string result = paragraph;
            if (results.languageToolResults is not null)
            {
                int startIndex = _ltItem!.StartIndexes[_paragraphs![_currentIndex].Id];
                foreach (var err in results.languageToolResults)
                {
                    // index = err.Index - _ltItem.StartIndexes[paragraph.Id]
                    result = result.Remove(err.Index - startIndex, err.Length);
                    result = result.Insert(err.Index - startIndex, err.Suggestion);
                }
            }
            if (results.prepositionCheckResults is not null)
            {
                foreach (var err in results.prepositionCheckResults)
                {
                    result = result.Replace(err.Error, err.Suggestion);
                }
            }
            return result;
        }   


        private void SetHeaderAndResult()
        {
            Header = "Kontrola pravopisu v dokumente...";
            TextResult = "Prebieha kontrola pravopisu, prosím neupravujte dokument počas prebiehajúcej kontroly...";
        }
        private void SetFinishResults()
        {
            Header = "Kontrola pravopisu prebehla";
            TextResult = "Kontrola pravopisu skončila, nenašli sa žiadne chyby.";
        }

    }
}
