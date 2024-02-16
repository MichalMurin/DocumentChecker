using CommonCode.DataServices;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class ConsistencyPage
    {
        [Inject]
        private ConsistencyPageDataService ConsistencyPageDataService { get; set; } = default!;


        protected override void OnInitialized()
        {
            Console.WriteLine($"Consistency page initialized {ConsistencyPageDataService.TitleConsistency}");
        }

        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/consistencyResult/{true}");
        }
    }
}
