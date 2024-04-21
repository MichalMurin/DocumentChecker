using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using DocumentChecker.JsConnectors;
using CommonCode.Services.DataServices;
using CommonCode.Deffinitions;
using System.Text.Json;

namespace DocumentChecker.Pages
{
    /// <summary>
    /// Represents the formatting page of the document checker.
    /// </summary>
    public partial class FormattingPage
    {
        [Inject]
        private FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        [Inject]
        public CommonJsConnectorService JsConnector { get; set; } = default!;

        /// <summary>
        /// The placeholder for the font name.
        /// </summary>
        private const string FORNT_NAME_PLACE_HOLDER = "Arial";

        /// <summary>
        /// Gets or sets a value indicating whether there is an error.
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Handles the click event when the import button is clicked.
        /// </summary>
        private async Task OnImportClick()
        {
            var x = Deffinitions.AlignmentDict.Values;
            Console.WriteLine(FormattingPageDataService.AllingmentDispalyed);
            Console.WriteLine("Import clicked - triggering file import");
            await JsConnector.TriggerImport("filePicker");
        }

        /// <summary>
        /// Handles the file import event.
        /// </summary>
        /// <param name="e">The event arguments containing the imported file.</param>
        private async Task ImportFile(InputFileChangeEventArgs e)
        {
            var file = e.File;
            long maxsize = 512000;
            var buffer = new byte[file.Size];
            await file.OpenReadStream(maxsize).ReadAsync(buffer);
            var fileContent = System.Text.Encoding.UTF8.GetString(buffer);
            var NewDataService = JsonSerializer.Deserialize<FormattingPageDataService>(fileContent);
            if (NewDataService is not null && ValidateData(NewDataService, "Nepodarilo sa načítať dáta zo súboru!"))
            {
                FormattingPageDataService.CopyFrom(NewDataService);
            }
        }

        /// <summary>
        /// Handles the click event when the export button is clicked.
        /// </summary>
        public async Task OnExportClick()
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var dataServiceShallowCopy = FormattingPageDataService.Clone();
            ((FormattingPageDataService)dataServiceShallowCopy).IgnoredParagraphs.Clear();
            var jsonContent = JsonSerializer.Serialize(dataServiceShallowCopy);
            writer.Write(jsonContent);
            writer.Flush();
            memoryStream.Position = 0;
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var url = $"data:application/octet-stream;base64,{base64}";
            var filename = "formatovanie.json";
            await JsConnector.SaveFile(url, filename);
        }

        /// <summary>
        /// Handles the click event when the start button is clicked.
        /// </summary>
        public override void OnStartClick()
        {
            // skontrolovat paragrafy
            if (ValidateData(FormattingPageDataService, "Nastavte prosím všetky hodnoty!"))
            {
                NavigationManager.NavigateTo($"/formattingResult/{true}");
            }
        }

        /// <summary>
        /// Validates the formatting page data.
        /// </summary>
        /// <param name="data">The formatting page data to validate.</param>
        /// <param name="errorMessage">The error message to display if validation fails.</param>
        /// <returns>True if the data is valid; otherwise, false.</returns>
        private bool ValidateData(FormattingPageDataService data, string errorMessage)
        {
            IsError = false;
            if (data.Heading1FontSize <= 0 ||
                data.Heading2FontSize <= 0 ||
                data.Heading3FontSize <= 0 ||
                data.Heading4FontSize <= 0 ||
                data.FontSize <= 0 ||
                string.IsNullOrEmpty(data.FontName) ||
                data.LineSpacing <= 0 ||
                data.LeftIndent < 0 ||
                data.LeftIndent < 0)
            {
                ShowError(errorMessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        private void ShowError(string message)
        {
            IsError = true;
            ErrorMessage = message;
        }

    }
}
