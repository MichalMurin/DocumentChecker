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
            Console.WriteLine("TitleConsistency: " + ConsistencyPageDataService.TitleConsistency);
            Console.WriteLine("CrossReferenceFunctionality: " + ConsistencyPageDataService.CrossReferenceFunctionality);
            Console.WriteLine("DocumentAlignment: " + ConsistencyPageDataService.DocumentAlignment);
            Console.WriteLine("DescriptionValidation: " + ConsistencyPageDataService.DescriptionValidation);
            Console.WriteLine("ListValidation: " + ConsistencyPageDataService.ListValidation);
            Console.WriteLine("InconsistentWordValidation: " + ConsistencyPageDataService.InconsistentWordValidation);
            Console.WriteLine("NumericalValidation: " + ConsistencyPageDataService.NumericalValidation);
            Console.WriteLine("ParenthesesValidation: " + ConsistencyPageDataService.ParenthesesValidation);
            Console.WriteLine("SentenceEndingValidation: " + ConsistencyPageDataService.SentenceEndingValidation);

            NavigationManager.NavigateTo($"/consistencyResult/{true}");
        }
    }
}
