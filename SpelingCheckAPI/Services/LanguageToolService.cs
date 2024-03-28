using CommonCode.ApiModels;
using CommonCode.CheckResults;
using SpelingCheckAPI.Interfaces;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SpelingCheckAPI.Services
{
    public class LanguageToolService: ILanguageToolService
    {
        public async Task<List<SpellingCheckResult>?> RunGrammarCheck(string text, List<string>? disabledRules = null)
        {
            string jasonResult;
            string tempFilePath = Path.GetTempFileName();

            try
            {
                // Write the input text to the temporary file
                await File.WriteAllTextAsync(tempFilePath, text);
                var disableRuleCommand = string.Empty;
                if (disabledRules is not null && disabledRules.Count > 0)
                {
                    disableRuleCommand = $"--disable {string.Join(",", disabledRules)}";
                }
                var processInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = $"-jar LanguageTool\\languagetool-commandline.jar -l sk-SK --encoding utf-8 {disableRuleCommand} --json {tempFilePath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8, // Set the encoding here
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();

                    // Read the output
                    string result = await process.StandardOutput.ReadToEndAsync();

                    // Optionally, you can handle result and error as needed
                    jasonResult = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
            finally
            {
                // Delete the temporary file
                File.Delete(tempFilePath);
            }
            var languageToolResult = JsonSerializer.Deserialize<LanguageToolApiResult>(jasonResult);
            return LanguageToolParser.TransformLtResultToCheckResult(languageToolResult);
        }

        ///////////////////////////
        ///
        //public async Task<LanguageToolResult?> RunGrammarCheckViaAPi(string text)
        //{
        //    // API endpoint
        //    string apiUrl = "https://api.languagetoolplus.com/v2/check";
        //    string result = string.Empty;
        //    // Create an instance of HttpClient
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Prepare form data
        //            var formData = new Dictionary<string, string>
        //        {
        //            { "text", text },
        //            { "language", "sk-SK" },
        //            { "enabledOnly", "false" }
        //        };

        //            // Create form content
        //            var formContent = new FormUrlEncodedContent(formData);

        //            // Make a POST request
        //            HttpResponseMessage response = await client.PostAsync(apiUrl, formContent);

        //            // Check if the request was successful (status code 200-299)
        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Read the response content as a string
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                result = apiResponse;
        //                // Process the API response
        //                Console.WriteLine(apiResponse);
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine($"Exception: {ex.Message}");
        //        }
        //        return JsonSerializer.Deserialize<LanguageToolResult>(result);
        //    }
        //}
    }
}
