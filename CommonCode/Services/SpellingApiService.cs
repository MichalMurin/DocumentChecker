using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.Results;
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

        public async Task<List<LanguageToolCheckResult>?> CheckLanguageTool(string text)
        {
            try
            {
                var res = await _httpClient.GetFromJsonAsync<List<LanguageToolCheckResult>>($"api/languageToolCheck/checkText/{text}");
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

        public async Task<List<PrepositionCheckResult>?> CheckPrepositions(string text)
        {
            try
            {
                var res = await _httpClient.GetFromJsonAsync<List<PrepositionCheckResult>>($"api/prepositionCheck/checkText/{text}");
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
    }
}
