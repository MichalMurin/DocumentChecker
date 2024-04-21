using CommonCode.Services.DataServices;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    /// <summary>
    /// Represents the ConsistencyPage component.
    /// </summary>
    public partial class ConsistencyPage
    {
        [Inject]
        private ConsistencyPageDataService ConsistencyPageDataService { get; set; } = default!;

        /// <summary>
        /// Handles the start click event.
        /// </summary>
        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/consistencyResult/{true}");
        }
    }
}
