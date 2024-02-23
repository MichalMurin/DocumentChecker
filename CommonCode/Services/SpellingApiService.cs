using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.Results;
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

        public async Task<List<LanguageToolCheckResult>?> CheckCmdLanguageTool(string text)
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
