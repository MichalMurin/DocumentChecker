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

        public async Task<APIResult<List<SpellingCheckResult>?>> CheckCmdLanguageTool(string text, List<string>? disabledRules = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Cannot check empty string for prepositions error!");
                return new APIResult<List<SpellingCheckResult>?>(null, false, "Cannot check empty string for LanguageTool errors!");
            }
            try
            {
                var model = new { Text = text, DisabledRules = disabledRules };
                var response = await _httpClient.PostAsJsonAsync("api/languageToolCheck/checkText", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<List<SpellingCheckResult>>();
                    return new APIResult<List<SpellingCheckResult>?>(result, true, null);
                }
                else
                {
                    Console.WriteLine($"Error occured during calling api for LanguageTool check: {response.StatusCode}");
                    return new APIResult<List<SpellingCheckResult>?>(null, false, $"Error occured during calling api for LanguageTool check: {response.StatusCode}");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException was thrown during calling api for LanguageTool check: {e}");
                return new APIResult<List<SpellingCheckResult>?>(null, false, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured during calling api for LanguageTool check: {e}");
                return new APIResult<List<SpellingCheckResult>?>(null, false, e.Message);
            }
        }

        public async Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositions(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Cannot check empty string for prepositions error!");
                return new APIResult<List<SpellingCheckResult>?>(null, false, "Cannot check empty string for prepositions error!");
            }
            try
            {
                var res = await _httpClient.GetFromJsonAsync<List<SpellingCheckResult>>($"api/prepositionCheck/checkText/{text}");
                return new APIResult<List<SpellingCheckResult>?>(res, true, null);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException was thrown during calling api for prepositions check: {e}");
                return new APIResult<List<SpellingCheckResult>?>(null, false, e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured during calling api for prepositions check: {e}");
                return new APIResult<List<SpellingCheckResult>?>(null, false, e.Message);
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
