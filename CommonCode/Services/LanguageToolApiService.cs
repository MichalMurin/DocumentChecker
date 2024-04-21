using CommonCode.ApiModels;
using CommonCode.CheckResults;
using CommonCode.Interfaces;
using CommonCode.ReturnValues;
using System.Text.Json;
using LanguageToolParagraph = (int startIndex, CommonCode.ApiModels.LanguageToolItem ltItem);

namespace CommonCode.Services
{
    /// <summary>
    /// Represents a service for interacting with the LanguageTool API.
    /// </summary>
    public class LanguageToolApiService : ILanguageToolApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseAddress = "https://api.languagetoolplus.com/v2/check";
        private const int MAX_NUMBER_OF_REQUEST_PER_MINUTE = 20;
        private const int MAX_NUMBER_OF_CHARACTERS_PER_MINUTE = 75_000;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageToolApiService"/> class.
        /// </summary>
        public LanguageToolApiService()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Checks the specified text using the LanguageTool API.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <returns>A list of spelling check results, or null if an error occurred.</returns>
        public async Task<List<SpellingCheckResult>?> CheckTextViaLanguageTool(string text)
        {
            string jsonResult = string.Empty;
            try
            {
                // Prepare form data
                var formData = new Dictionary<string, string>
                    {
                        { "text", text },
                        { "language", "sk-SK" },
                        { "enabledOnly", "false" }
                    };

                // Create form content
                var formContent = new FormUrlEncodedContent(formData);

                // Make a POST request
                HttpResponseMessage response = await _httpClient.PostAsync(_apiBaseAddress, formContent);

                // Check if the request was successful (status code 200-299)
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    jsonResult = apiResponse;
                    // Process the API response
                    Console.WriteLine(apiResponse);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            var ltResult = JsonSerializer.Deserialize<LanguageToolApiResult>(jsonResult);
            return LanguageToolParser.TransformLtResultToCheckResult(ltResult);
        }

        /// <summary>
        /// Creates LanguageTool items for the specified paragraphs.
        /// </summary>
        /// <param name="paragraphs">The list of paragraphs.</param>
        /// <returns>A dictionary containing LanguageTool paragraphs.</returns>
        public Dictionary<string, LanguageToolParagraph> CreateLanguageToolItems(List<ParagraphData> paragraphs)
        {
            var lTItems = new Dictionary<string, LanguageToolParagraph>();
            int maxItemLength = MAX_NUMBER_OF_CHARACTERS_PER_MINUTE / MAX_NUMBER_OF_REQUEST_PER_MINUTE;
            string tmpText = string.Empty;
            var ltItem = new LanguageToolItem();
            foreach (var para in paragraphs)
            {
                if (tmpText != string.Empty && tmpText.Length + para.Text.Length > maxItemLength)
                {
                    ltItem = new LanguageToolItem();
                    tmpText = string.Empty;
                }
                // saving at key = paragraph ID => (start index of paragraph, LanguageToolItem)
                lTItems[para.Id] = (tmpText.Length, ltItem);
                tmpText += para.Text;
                ltItem.NumberOfParagraphs++;
                ltItem.Text = tmpText;
            }
            return lTItems;
        }
    }
}
