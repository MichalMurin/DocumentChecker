using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using static CommonCode.Deffinitions.Deffinitions;
namespace DocumentChecker.Pages.ResultPages
{
    /// <summary>
    /// Represents the spelling result page.
    /// </summary>
    public partial class SpellingResultPage : BaseResultPage
    {
        [Inject]
        public SpellingPageConnectorService JsConnector { get; set; } = default!;
        [Inject]
        public ISpellingApiService SpellingApiService { get; set; } = default!;

        /// <summary>
        /// Gets or sets the data service for the spelling page.
        /// </summary>
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
        private readonly List<SpellingCheckResult> _currentSpellingResult = [];

        /// <summary>
        /// Gets the scan result for the spelling page.
        /// </summary>
        /// <param name="isStart">Indicates if the scan is starting.</param>
        /// <returns>The scan result.</returns>
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

        /// <summary>
        /// Handles the API spelling check for a paragraph.
        /// </summary>
        /// <param name="resultsList">The list to store the spelling check results.</param>
        /// <param name="paragraph">The paragraph to check.</param>
        /// <param name="checkFunction">The function to perform the spelling check.</param>
        /// <returns>True if the spelling check is successful, false otherwise.</returns>
        private static async Task<bool> HandleApiSpellingCheck(List<SpellingCheckResult> resultsList, ParagraphData paragraph,
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

        /// <summary>
        /// Starts the spelling check for a paragraph.
        /// </summary>
        /// <param name="paragraphIndex">The index of the paragraph to start the check.</param>
        /// <returns>The scan result.</returns>
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
                        tmpList = CheckOwnRules(_paragraphs[i]);
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

        /// <summary>
        /// Fills the errors in the data service.
        /// </summary>
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

        /// <summary>
        /// Checks the language tool in a paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph to check.</param>
        /// <returns>The API result of the language tool check.</returns>
        private async Task<APIResult<List<SpellingCheckResult>?>> CheckLanguageToolInParagraph(ParagraphData paragraph)
        {
            _ltItem ??= SpellingApiService.CreateLanguageToolItem(_paragraphs!);
            var startIndex = _ltItem.StartIndexes[paragraph.Id];
            var languageToolResults = _ltItem.Result;
            if (languageToolResults is null)
            {
                var apiResult = await SpellingApiService.CheckCmdLanguageTool(_ltItem.Text, DataService.LanguageToolPriority, DataService.LanguageToolDisabledRules);
                if (apiResult.IsSuccess)
                {
                    _ltItem.Result = apiResult.Result;
                }
                else
                {
                    return apiResult;
                }
                languageToolResults = _ltItem.Result ?? [];
            }
            var relevantCheckResults = languageToolResults?.Where(x => x.Index >= startIndex && x.Index <= startIndex + paragraph.Text.Length).ToList();
            relevantCheckResults?.ForEach(x => x.Index -= startIndex);
            return new APIResult<List<SpellingCheckResult>?>(relevantCheckResults, true, null);
        }

        /// <summary>
        /// Checks the own rules in a paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph to check.</param>
        /// <returns>The list of spelling check results.</returns>
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
                        foreach (System.Text.RegularExpressions.Match match in matches.Cast<System.Text.RegularExpressions.Match>())
                        {
                            Console.WriteLine($"Nasla sa zhoda: {match.Value}");
                            var ownRuleCheckResult = new SpellingCheckResult
                            {
                                Priority = DataService.OwnRulesPriority,
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

        /// <summary>
        /// Checks the prepositions in a paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph to check.</param>
        /// <returns>The API result of the preposition check.</returns>
        private async Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositionInParagraph(ParagraphData paragraph)
        {
            return await SpellingApiService.CheckPrepositions(text: paragraph.Text, DataService.PrepositionCheckPriority);
        }

        /// <summary>
        /// Corrects the paragraph based on the spelling check results.
        /// </summary>
        /// <param name="paragraph">The paragraph to correct.</param>
        /// <param name="checkResults">The spelling check results.</param>
        /// <returns>The corrected paragraph.</returns>
        private static string CorrectParagraph(string paragraph, List<SpellingCheckResult> checkResults)
        {
            string result = paragraph;
            // Group errors by index and sort groups by index in descending order
            var groupedErrors = checkResults.GroupBy(e => e.Index).OrderByDescending(g => g.Key);
            foreach (var group in groupedErrors)
            {
                // Sort errors in each group by priority and correct only error with highest priority
                var errors = group.OrderBy(e => e.Priority);
                var err = errors.First();
                if (err.Index + err.Length > paragraph.Length)
                {
                    err.Length = paragraph.Length - err.Index;
                    err.Suggestion = err.Suggestion[..err.Length];
                }
                result = result.Remove(err.Index, err.Length).Insert(err.Index, err.Suggestion);
            }
            return result;
        }

        /// <summary>
        /// Tries to correct the paragraph based on the found errors.
        /// </summary>
        /// <returns>True if the paragraph is corrected, false otherwise.</returns>
        protected override async Task<bool> TryToCorrectParagraph()
        {
            if (_paragraphs is not null)
            {
                var errorsToCorect = DataService.FoundErrors.Select(err => ((FoundSpellingErrorModel)err).SpellingCheckResult).ToList();
                if (errorsToCorect is not null)
                {
                    await JsConnector.ReplaceSelectedText(CorrectParagraph(_paragraphs[_currentIndex].Text, errorsToCorect));
                }
                // returning true even if there was no error to correct
                return true;
            }
            else
            {
                return false;
            }
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

        /// <summary>
        /// Shows the API error message.
        /// </summary>
        private void ShowApiErrorMessage()
        {
            DataService.ErrorMessage = "Nastala chyba pri komunikácii s API serverom!";
            DataService.IsError = true;
        }

        /// <summary>
        /// Hides the error message.
        /// </summary>
        private void HideError()
        {
            DataService.IsError = false;
        }
    }
}
