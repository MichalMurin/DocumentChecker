using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    /// <summary>
    /// Represents a base class for pages.
    /// </summary>
    public abstract class BaseJsPage : ComponentBase
    {
        /// <summary>
        /// Called when the start button is clicked.
        /// </summary>
        public abstract void OnStartClick();
    }
}
