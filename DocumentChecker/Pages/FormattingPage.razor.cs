using CommonCode.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using DocumentChecker.JsConnectors;
using CommonCode.Services.DataServices;
using CommonCode.Formatting;

namespace DocumentChecker.Pages
{
    public partial class FormattingPage
    {

        private const string FORNT_NAME_PLACE_HOLDER  = "Arial";

        //private const string ALLIGMENT_PLACEHOLDER = "Justified";
        [Inject]
        private FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        [Inject]
        public CommonJsConnectorService JsConnector { get; set; } = default!;

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
            Console.WriteLine(fileContent + " " + file.Size);
            await JsConnector.InsertTextToWord(fileContent);
        }
        public async Task OnExportClick()
        {
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);

            writer.Write("Your file content goes here");
            writer.Flush();
            memoryStream.Position = 0;
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var url = $"data:application/octet-stream;base64,{base64}";
            var filename = "testFile.txt";
            await JsConnector.SaveFile(url, filename);
        }
        public override void OnStartClick()
        {
            // skontrolovat paragrafy
            NavigationManager.NavigateTo($"/formattingResult/{true}");
        }
    }
}
