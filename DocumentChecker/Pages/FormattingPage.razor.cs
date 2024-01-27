using DocumentChecker.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace DocumentChecker.Pages
{
    public partial class FormattingPage
    {
        private int _currentCount = 0;

        public FormattingPage(): base("./Pages/FormattingPage.razor.js")
        {            
        }

        private async Task GetNumberOfWords()
        {
            ReturnValue<string> ret = await JSModule.InvokeAsync<ReturnValue<string>>("GetAllText");
            var words = ret.Value.Trim().Split(" ");
            _currentCount = words.Count();
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
