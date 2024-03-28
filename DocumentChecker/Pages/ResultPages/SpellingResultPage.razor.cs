using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Dynamic;
using System.Text.RegularExpressions;
using static CommonCode.Deffinitions.Deffinitions;
namespace DocumentChecker.Pages.ResultPages
{
    public partial class SpellingResultPage: BaseResultPage
    {
        [Inject]
        public SpellingPageConnectorService JsConnector { get; set; } = default!;
        [Inject]
        public ISpellingApiService SpellingApiService { get; set; } = default!;
        protected override SpellingPageDataService DataService
        {
            get
            {
                return DataServiceFactory.GetSpellingDataService();
            }
        }

        private List<ParagraphData>? _paragraphs;
        private LanguageToolItem? _ltItem;
        private int _currentIndex = 0;
        private List<SpellingCheckResult> _currentSpellingResult = new List<SpellingCheckResult>();

        protected override async Task<ScanReturnValue> GetScanResult(bool isStart)
        {
            HideError();
            if (isStart)
            {
                _paragraphs = await JsConnector.GetParagrapghs();
                _currentIndex = 0;
            }
            else
            {
                _currentIndex++;
            }
            return await StartCheck(_currentIndex);
        }

        private async Task<bool> HandleApiSpellingCheck(List<SpellingCheckResult> resultsList, ParagraphData paragraph,
            Func<ParagraphData, Task<APIResult<List<SpellingCheckResult>?>>> checkFunction)
        {
            var apiResult = await checkFunction(paragraph);
            if (apiResult.IsSuccess)
            {
                var tmpList = apiResult.Result;
                if (tmpList is not null)
                {
                    resultsList.AddRange(tmpList);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<ScanReturnValue> StartCheck(int paragraphIndex)
        {
            if (_paragraphs is not null && paragraphIndex < _paragraphs.Count)
            {
                var tmpList = new List<SpellingCheckResult>();
                for (int i = paragraphIndex; i < _paragraphs.Count; i++)
                {
                    if (string.IsNullOrEmpty(_paragraphs[i].Text))
                    {
                        // We dont have to check empty string
                        continue;
                    }
                    _currentSpellingResult.Clear();
                    if (DataService.CheckPrepositions && !(await HandleApiSpellingCheck(_currentSpellingResult, _paragraphs[i], CheckPrepositionInParagraph)))
                    {
                        ShowApiErrorMessage();
                        break;
                    }

                    if (DataService.CheckLanguageTool && !(await HandleApiSpellingCheck(_currentSpellingResult, _paragraphs[i], CheckLanguageToolInParagraph)))
                    {
                        ShowApiErrorMessage();
                        break;
                    }

                    if (DataService.Rules.Count > 0 && DataService.CheckOwnRules)
                    {
                        tmpList =  CheckOwnRules(_paragraphs[i]);
                        if (tmpList is not null)
                        {
                            _currentSpellingResult.AddRange(tmpList);
                        }
                    }

                    if (_currentSpellingResult.Count > 0)
                    {
                        await JsConnector.SelectParagraph(i);
                        _currentIndex = i;
                        return new ScanReturnValue
                        {
                            FoundError = true,
                            ParagraphId = _paragraphs[i].Id
                        };
                    }
                }
            }
            return new ScanReturnValue
            {
                FoundError = false
            };
        }

        protected override void FillErrors()
        {
            if (_currentSpellingResult.Count > 0)
            {
                foreach (var err in _currentSpellingResult)
                {
                    var message = err.ShortMessage;
                    if (string.IsNullOrEmpty(message))
                    {
                        message = err.Message;
                    }
                    DataService.FoundErrors.Add(
                        new FoundSpellingErrorModel()
                        {
                            Name = $"{message}: {err.ErrorSentence}",
                            Description = $"Oprava: {err.Suggestion}",
                            SpellingCheckResult = err
                        }
                    );
                }
            }
        }

        private async Task<APIResult<List<SpellingCheckResult>?>> CheckLanguageToolInParagraph(ParagraphData paragraph)
        {
            if (_ltItem is null)
            {
                _ltItem = SpellingApiService.CreateLanguageToolItem(_paragraphs!);
            }
            var startIndex = _ltItem.StartIndexes[paragraph.Id];
            var languageToolResults = _ltItem.Result;
            if (languageToolResults is null)
            {
                var apiResult = await SpellingApiService.CheckCmdLanguageTool(_ltItem.Text, DataService.LanguageToolDisabledRules);
                if (apiResult.IsSuccess)
                {
                    _ltItem.Result = apiResult.Result;
                }
                else
                {
                    return apiResult;
                }
                languageToolResults = _ltItem.Result ?? new List<SpellingCheckResult>();
            }
            var relevantCheckResults = languageToolResults?.Where(x => x.Index >= startIndex && x.Index <= startIndex + paragraph.Text.Length).ToList();
            if (relevantCheckResults is not null)
            {
                relevantCheckResults.ForEach(x => x.Index -= startIndex);
            }
            return new APIResult<List<SpellingCheckResult>?>(relevantCheckResults, true, null);
        }

        private List<SpellingCheckResult>? CheckOwnRules(ParagraphData paragraph)
        {
            if (DataService.Rules.Count > 0)
            {
                var result = new List<SpellingCheckResult>();
                foreach (var rule in DataService.Rules)
                {
                    var regex = rule.RegexRule;
                    var matches = Regex.Matches(paragraph.Text, regex);
                    if (matches.Count > 0)
                    {
                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            Console.WriteLine($"Nasla sa zhoda: {match.Value}");
                            var ownRuleCheckResult = new SpellingCheckResult
                            {
                                Message = rule.Description,
                                ShortMessage = rule.Description,
                                Suggestion = rule.Correction,
                                Index = match.Index,
                                Length = match.Length
                            };
                            result.Add(ownRuleCheckResult);
                        }
                    }
                }
                return result;
            }
            return null;
        }


        private async Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositionInParagraph(ParagraphData paragraph)
        {
            return await SpellingApiService.CheckPrepositions(text: paragraph.Text);
            
        }

        private string CorrectParagraph(string paragraph, List<SpellingCheckResult> checkResults)
        {
            string result = paragraph;
            foreach (var err in checkResults.OrderByDescending(x => x.Index))
            {
                result = result.Remove(err.Index, err.Length).Insert(err.Index, err.Suggestion);
                //result = result.Replace(err.ErrorSentence, err.Suggestion);
            }
            return result;
        }   

        protected override async Task<bool> TryToCorrectParagraph()
        {
            if (_paragraphs is not null)
            {
                var errorsToCorect = DataService.FoundErrors.Select(err => ((FoundSpellingErrorModel)err).SpellingCheckResult).ToList();
                if (errorsToCorect is not null)
                {
                    await JsConnector.ReplaceSelectedText(CorrectParagraph(_paragraphs[_currentIndex].Text, errorsToCorect));
                }
                // retruning true even there was no error to correct
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void SetDisplayedTexts(CheckState state)
        {
            switch (state)
            {
                case CheckState.FOUND_ERROR:
                    Header = "Našla sa chyba!";
                    TextResult = $"Boli zistené chyby v pravopise. Aby bola oprava úspešná, prosím, nechajte odstavec označený";
                    break;
                case CheckState.FINISHED:
                    Header = "Kontrola dokončená!";
                    TextResult = $"Kontrola bola úspešne ukončená, nenašli sa žiadne ďalšie chyby v pravopise";
                    break;
                default:
                    base.SetDisplayedTexts(state);
                    break;
            }
        }

        private void ShowApiErrorMessage()
        {
            DataService.ErrorMessage = "Nastala chyba pri komunikácii s API serverom!";
            DataService.IsError = true;
        }

        private void HideError()
        {
            DataService.IsError = false;
        }


    }
}
