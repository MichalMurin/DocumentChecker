using CommonCode.Services.DataServices;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    /// <summary>
    /// Represents the spelling page of the document checker.
    /// </summary>
    public partial class SpellingPage
    {
        [Inject]
        private SpellingPageDataService SpellingPageDataService { get; set; } = default!;

        /// <summary>
        /// Opens the new rule page.
        /// </summary>
        private void OpenNewRulePage()
        {
            NavigationManager.NavigateTo("/newrule");
        }

        /// <summary>
        /// Handles the start click event.
        /// </summary>
        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/spellingResult/{true}");
        }
    }
}
