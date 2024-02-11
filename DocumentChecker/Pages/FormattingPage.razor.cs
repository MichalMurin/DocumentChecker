using CommonCode.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using CommonCode.DataServices;
using DocumentChecker.JsConnectors;

namespace DocumentChecker.Pages
{
    public partial class FormattingPage
    {

        private const string FORNT_NAME_PLACE_HOLDER  = "Times New Roman";

        private const string ALLIGMENT_PLACEHOLDER = "Justified";
        [Inject]
        private FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        [Inject]
        public CommonJsConnectorService JsConnector { get; set; } = default!;

        private async Task OnImportClick()
        {
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








        //private async Task GetNumberOfWords()
        //{
        //    ReturnValue<string> ret = await JSModule.InvokeAsync<ReturnValue<string>>("GetAllText");
        //    var words = ret.Value.Trim().Split(" ");
        //    //_currentCount = words.Count();
        //    //await JSModule.InvokeVoidAsync($"InsertText", $"Current count is {_currentCount}");
        //}

        //private async Task GetWordInfo()
        //{
        //    ReturnValue<List<WordInfo>> ret = await JSModule.InvokeAsync<ReturnValue<List<WordInfo>>>("GetWordInfos");
        //    if (ret.Value == null)
        //    {
        //        Console.WriteLine($"Value is null");
        //        return;
        //    }
        //    foreach (var wordInfo in ret.Value)
        //    {
        //        Console.WriteLine($"Text: {wordInfo.Text}, info: {wordInfo.Font.Name} - {wordInfo.Font.Color}");
        //        await JSModule.InvokeVoidAsync("InsertText", $"Text: {wordInfo.Text}, info: {wordInfo.Font.Name} - {wordInfo.Font.Color}\n");
        //    }
            
        //    //await JSModule.InvokeVoidAsync($"InsertText", $"Current count is {_currentCount}");
        //}
    }
}
