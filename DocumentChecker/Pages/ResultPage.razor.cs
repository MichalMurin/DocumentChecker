using CommonCode.DataServices;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class ResultPage
    {
        //https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-8.0
        [Inject]
        private FormattingPageDataService FormattingPageDataService { get; set; } = default!;
        public string Result { get; set; } = "Kontroluje sa dokument...";
        public void OnIgnoreClick()
        {
            Result = "Chyba bola ignorovaná";
        }
        public void OnCorrectClick()
        {
            Result = "Chyba bola opravená";
        }
    }
}
