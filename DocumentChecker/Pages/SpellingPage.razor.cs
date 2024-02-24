using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class SpellingPage
    {
        [Inject]
        private SpellingPageDataService SpellingPageDataService { get; set; } = default!;

        private void OpenNewRulePage()
        {
            NavigationManager.NavigateTo("/newrule");
        }

        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/spellingResult/{true}");
        }
    }
}
