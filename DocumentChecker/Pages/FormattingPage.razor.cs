using DocumentChecker.ReturnValues;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
    }
}
