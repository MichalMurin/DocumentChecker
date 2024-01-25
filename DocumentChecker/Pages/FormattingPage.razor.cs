using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DocumentChecker.Pages
{
    public class ReturnValue
    {
        public string Value { get; set; } = string.Empty;
    }
    public partial class FormattingPage
    {
        private int _currentCount = 0;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;
        public IJSObjectReference JSModule { get; set; } = default!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/FormattingPage.razor.js");
            }
        }

        private async Task GetNumberOfWords()
        {
            ReturnValue ret = await JSModule.InvokeAsync<ReturnValue>("GetAllText");
            var words = ret.Value.Split(" ");
            _currentCount = words.Count();
            //await JSModule.InvokeVoidAsync($"InsertText", $"Current count is {_currentCount}");
        }
    }
}
