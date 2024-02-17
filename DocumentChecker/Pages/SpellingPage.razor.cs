using DocumentChecker.JsConnectors;
using LanguageToolHandler.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class SpellingPage
    {
        [Inject]
        public SpellingPageConnectorService JsConnector { get; set; } = default!;
        [Inject]
        private ILanguageToolService LanguageToolService { get; set; } = default!;
        private void OpenNewRulePage()
        {
            NavigationManager.NavigateTo("/newrule");
        }
        public async Task OnStartClickAsync()
        {
            var paragraphs = await JsConnector.GetParagrapghs();

            foreach (var paragraph in paragraphs)
            {
                var res = await LanguageToolService.RunGrammarCheckViaAPi(paragraph.Text);
                Console.WriteLine(paragraph.Id);
                Console.WriteLine(paragraph.Text);
                Console.WriteLine(res);
            }
            //NavigationManager.NavigateTo($"/spellingResult/{true}");
        }

        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/spellingResult/{true}");
        }
    }
}
