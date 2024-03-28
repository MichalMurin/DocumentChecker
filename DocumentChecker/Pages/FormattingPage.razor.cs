using CommonCode.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using DocumentChecker.JsConnectors;
using CommonCode.Services.DataServices;
using CommonCode.Deffinitions;
using System.Text.Json;

namespace DocumentChecker.Pages
{
    public partial class FormattingPage
    {
        [Inject]
        private FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        [Inject]
        public CommonJsConnectorService JsConnector { get; set; } = default!;
        private const string FORNT_NAME_PLACE_HOLDER = "Arial";
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        private async Task OnImportClick()
        {
            var x = Deffinitions.AlignmentDict.Values;
            Console.WriteLine(FormattingPageDataService.AllingmentDispalyed);
            Console.WriteLine("Import clicked - triggering file import");
            await JsConnector.TriggerImport("filePicker");
        }
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
        public override void OnStartClick()
        {
            // skontrolovat paragrafy
            if (ValidateData(FormattingPageDataService, "Nastavte prosím všetky hodnoty!"))
            {
                NavigationManager.NavigateTo($"/formattingResult/{true}");
            }
        }

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

        private void ShowError(string message)
        {
            IsError = true;
            ErrorMessage = message;
        }

    }
}
