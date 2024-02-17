using LanguageToolHandler.Interfaces;
using System.Diagnostics;

namespace LanguageToolHandler
{
    public class LanguageToolService : ILanguageToolService
    {
        public async Task<string> RunGrammarCheck(string text)
        {
            string output;
            string tempFilePath = Path.GetTempFileName();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string outputFilePath = Path.Combine(desktopPath, "output.txt");

            try
            {
                // Write the input text to the temporary file
                await File.WriteAllTextAsync(tempFilePath, text);

                var processInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = $"-jar C:\\Windows\\System32\\cmd.exe\\languagetool-commandline.jar -l sk-SK --json {tempFilePath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();

                    // Read the output
                    string result = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    // Optionally, you can handle result and error as needed
                    if (!string.IsNullOrEmpty(error))
                    {
                        output = "Error: " + error;
                    }
                    else
                    {
                        output = "Output: " + result;
                    }
                }

                // Save the output to a file on the desktop
                await File.WriteAllTextAsync(outputFilePath, output);
            }
            catch (Exception ex)
            {
                output = "Exception: " + ex.Message;
            }
            finally
            {
                // Delete the temporary file
                File.Delete(tempFilePath);
            }

            return output;
        }

        public async Task<string> RunGrammarCheckViaAPi(string text)
        {
            // API endpoint
            string apiUrl = "https://api.languagetoolplus.com/v2/check";
            string result = string.Empty;
            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
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
                    HttpResponseMessage response = await client.PostAsync(apiUrl, formContent);

                    // Check if the request was successful (status code 200-299)
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = apiResponse;
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
                return result;
            }
        }
    }
}
