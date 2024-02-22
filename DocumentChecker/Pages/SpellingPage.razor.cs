using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;

namespace DocumentChecker.Pages
{
    public partial class SpellingPage
    {
        private void OpenNewRulePage()
        {
            NavigationManager.NavigateTo("/newrule");
        }
        //public async Task OnStartClickAsync()
        //{
        //    var paragraphs = await JsConnector.GetParagrapghs();



        //    foreach (var paragraph in paragraphs)
        //    {
        //        //var res = await LanguageToolService.RunGrammarCheckViaAPi(paragraph.Text);
        //        //Console.WriteLine(paragraph.Id);
        //        //Console.WriteLine(paragraph.Text);
        //        //Console.WriteLine(res);

        //        //var res = await PrepositionCheckService.CheckPrepositionsInText(paragraph.Text);
        //        //foreach (var item in res)
        //        //{
        //        //    Console.WriteLine($"Zla predlozka: {item.Error}");
        //        //    Console.WriteLine($"Navrhovana oprava: {item.Suggestion}");
        //        //}
        //    }
        //    //NavigationManager.NavigateTo($"/spellingResult/{true}");
        //}

        public override void OnStartClick()
        {
            NavigationManager.NavigateTo($"/spellingResult/{true}");
        }
    }
}
