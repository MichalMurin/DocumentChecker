using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DocumentChecker.Pages
{
    public abstract class BaseJsPage: ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;
        public IJSObjectReference JSModule { get; set; } = default!;
        public string JsModulePath { get; set; }

        public BaseJsPage(string jsModulePath) : base()
        {
            JsModulePath = jsModulePath;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath);
            }
        }
    }
}
