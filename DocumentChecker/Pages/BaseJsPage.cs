using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DocumentChecker.Pages
{
    public abstract class BaseJsPage: ComponentBase
    {
        public abstract void OnStartClick();
    }
}
