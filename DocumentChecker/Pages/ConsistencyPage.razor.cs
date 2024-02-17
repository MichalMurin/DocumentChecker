using CommonCode.DataServices;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class ConsistencyPage
    {
        [Inject]
        private ConsistencyPageDataService ConsistencyPageDataService { get; set; } = default!;

        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/consistencyResult/{true}");
        }
    }
}
