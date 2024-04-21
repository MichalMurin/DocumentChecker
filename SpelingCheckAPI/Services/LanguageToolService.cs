using CommonCode.ApiModels;
using CommonCode.CheckResults;
using SpelingCheckAPI.Interfaces;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace SpelingCheckAPI.Services
{
    /// <summary>
    /// Service class for running grammar checks using LanguageTool.
    /// </summary>
    public class LanguageToolService : ILanguageToolService
    {
        /// <summary>
        /// Runs a grammar check on the given text.
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <param name="disabledRules">Optional list of disabled rules.</param>
        /// <returns>A list of spelling check results.</returns>
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
    }
}
