using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.ReturnValues;
using System.Net.Http.Json;

namespace CommonCode.Services
{
    public class SpellingApiService: ISpellingApiService
    {
        /// <summary>
        /// Http klient pre posielanie API requestov
        /// </summary>
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseAddress = "https://localhost:7200";

        public SpellingApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new System.Uri(_apiBaseAddress);
        }

        public async Task<List<SpellingCheckResult>?> CheckCmdLanguageTool(string text, List<string>? disabledRules = null)
        {
            try
            {
                var disabledRulesQuerry = string.Empty;
                if (disabledRules is not null && disabledRules.Count > 0)
                {
                    disabledRulesQuerry = $"?{string.Join("&", disabledRules.Select(rule => $"disabledRules={Uri.EscapeDataString(rule)}"))}";
                }
                var res = await _httpClient.GetFromJsonAsync<List<SpellingCheckResult>>($"api/languageToolCheck/checkText/{text}{disabledRulesQuerry}");
                return res;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException was thrown during calling api for LanguageTool check: {e}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured during calling api for LanguageTool check: {e}");
                throw;
            }
        }

        public async Task<List<SpellingCheckResult>?> CheckPrepositions(string text)
        {
            try
            {
                var res = await _httpClient.GetFromJsonAsync<List<SpellingCheckResult>>($"api/prepositionCheck/checkText/{text}");
                return res;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException was thrown during calling api for prepositions check: {e}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured during calling api for prepositions check: {e}");
                return null;
            }
        }

        public LanguageToolItem CreateLanguageToolItem(List<ParagraphData> paragraphs)
        {
            string tmpText = string.Empty;
            var ltItem = new LanguageToolItem();
            foreach (var para in paragraphs)
            {
                ltItem.StartIndexes[para.Id] = tmpText.Length;
                tmpText += para.Text;
            }
            ltItem.Text = tmpText;
            ltItem.NumberOfParagraphs = paragraphs.Count;
            return ltItem;
        }
    }
}
