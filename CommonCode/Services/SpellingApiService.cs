using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.ReturnValues;
using System.Net.Http.Json;
using System.Text;

namespace CommonCode.Services
{
    /// <summary>
    /// Service for interacting with the Spelling API.
    /// </summary>
    public class SpellingApiService : ISpellingApiService
    {
        /// <summary>
        /// Http client for sending API requests.
        /// </summary>
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseAddress = "https://localhost:7200";

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellingApiService"/> class.
        /// </summary>
        public SpellingApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_apiBaseAddress)
            };
        }

        /// <summary>
        /// Processes a successful API response.
        /// </summary>
        /// <param name="response">The HTTP response message.</param>
        /// <param name="priority">The priority of the spelling check.</param>
        /// <returns>The API result containing the spelling check results.</returns>
        private static async Task<APIResult<List<SpellingCheckResult>?>> ProcessSuccessfulRepsonse(HttpResponseMessage response, int priority)
        {
            var result = await response.Content.ReadFromJsonAsync<List<SpellingCheckResult>>();
            result?.ForEach(item => item.Priority = priority);
            return new APIResult<List<SpellingCheckResult>?>(result, true, null);
        }

        /// <summary>
        /// Checks the given text using the LanguageTool API.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="priority">The priority of the spelling check.</param>
        /// <param name="disabledRules">The list of disabled rules.</param>
        /// <returns>The API result containing the spelling check results.</returns>
        public async Task<APIResult<List<SpellingCheckResult>?>> CheckCmdLanguageTool(string text, int priority, List<string>? disabledRules = null)
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
                    return await ProcessSuccessfulRepsonse(response, priority);
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

        /// <summary>
        /// Checks the given text for prepositions.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="priority">The priority of the spelling check.</param>
        /// <returns>The API result containing the spelling check results.</returns>
        public async Task<APIResult<List<SpellingCheckResult>?>> CheckPrepositions(string text, int priority)
        {
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("Cannot check empty string for prepositions error!");
                return new APIResult<List<SpellingCheckResult>?>(null, false, "Cannot check empty string for prepositions error!");
            }
            try
            {
                var model = new { Text = text };
                var response = await _httpClient.PostAsJsonAsync("api/prepositionCheck/checkText", model);

                if (response.IsSuccessStatusCode)
                {
                    return await ProcessSuccessfulRepsonse(response, priority);
                }
                else
                {
                    Console.WriteLine($"Error occured during calling api for Prepositions check: {response.StatusCode}");
                    return new APIResult<List<SpellingCheckResult>?>(null, false, $"Error occured during calling api for Preopsitions check: {response.StatusCode}");
                }
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

        /// <summary>
        /// Creates a LanguageTool item from the given paragraphs.
        /// </summary>
        /// <param name="paragraphs">The list of paragraphs.</param>
        /// <returns>The created LanguageTool item.</returns>
        public LanguageToolItem CreateLanguageToolItem(List<ParagraphData> paragraphs)
        {
            var stringBuilder = new StringBuilder();
            var ltItem = new LanguageToolItem();
            foreach (var para in paragraphs)
            {
                ltItem.StartIndexes[para.Id] = stringBuilder.Length;
                stringBuilder.Append(para.Text);
            }
            ltItem.Text = stringBuilder.ToString();
            ltItem.NumberOfParagraphs = paragraphs.Count;
            return ltItem;
        }
    }
}
