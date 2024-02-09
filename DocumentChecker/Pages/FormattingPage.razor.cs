using DocumentChecker.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
using System.Text.Json;

namespace DocumentChecker.Pages
{
    public partial class FormattingPage
    {
        private List<string> _ignoredParagraphsIds = new List<string>();
        private int _font_size;
        private string _font_name = string.Empty;
        private string _alligment = string.Empty;
        private double _lineSpacing;
        private int _leftIndent;
        private int _rightIndent;

        public FormattingPage(): base("./Pages/FormattingPage.razor.js")
        {            
        }
        private async Task OnImportClick()
        {
            Console.WriteLine("Import clicked - triggering file import");
            await JSRuntime.InvokeVoidAsync("TriggerImport", "filePicker");
        }
        private async Task ImportFile(InputFileChangeEventArgs e)
        {
            var file = e.File;
            long maxsize = 512000;
            var buffer = new byte[file.Size];
            await file.OpenReadStream(maxsize).ReadAsync(buffer);
            var fileContent = System.Text.Encoding.UTF8.GetString(buffer);
            Console.WriteLine(fileContent + " " + file.Size);
            await JSModule.InvokeVoidAsync("InsertText", fileContent);
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

            await JSRuntime.InvokeVoidAsync("saveAsFile", url, filename);
        }
        public override async void OnStartClick()
        {
            // skontrolovat paragrafy
            // potom skontorlovat hlavicky a paty
            // riakdovanie musime nastavit ako: velkost fontu * riadkovanie =  pocet bodov pre vysku riadke (12* 1.5 = 18)
            NavigationManager.NavigateTo("/result");
            await JSModule.InvokeVoidAsync("checkParagraphs", _ignoredParagraphsIds, _font_name, _font_size, _alligment, _lineSpacing * _font_size, _leftIndent, _rightIndent);
        }








        private async Task GetNumberOfWords()
        {
            ReturnValue<string> ret = await JSModule.InvokeAsync<ReturnValue<string>>("GetAllText");
            var words = ret.Value.Trim().Split(" ");
            //_currentCount = words.Count();
            //await JSModule.InvokeVoidAsync($"InsertText", $"Current count is {_currentCount}");
        }

        private async Task GetWordInfo()
        {
            ReturnValue<List<WordInfo>> ret = await JSModule.InvokeAsync<ReturnValue<List<WordInfo>>>("GetWordInfos");
            if (ret.Value == null)
            {
                Console.WriteLine($"Value is null");
                return;
            }
            foreach (var wordInfo in ret.Value)
            {
                Console.WriteLine($"Text: {wordInfo.Text}, info: {wordInfo.Font.Name} - {wordInfo.Font.Color}");
                await JSModule.InvokeVoidAsync("InsertText", $"Text: {wordInfo.Text}, info: {wordInfo.Font.Name} - {wordInfo.Font.Color}\n");
            }
            
            //await JSModule.InvokeVoidAsync($"InsertText", $"Current count is {_currentCount}");
        }
    }
}
