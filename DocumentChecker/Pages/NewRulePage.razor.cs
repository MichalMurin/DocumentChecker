using CommonCode.Models;
using CommonCode.Services.DataServices;
using DocumentChecker.JsConnectors;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace DocumentChecker.Pages
{
    public partial class NewRulePage
    {
        [Inject]
        private SpellingPageDataService SpellingPageDataService { get; set; } = default!;
        private string Rule { get; set; } = string.Empty;
        public string DescriptionRule { get; set; } = "Vložte vlastné RegEx pravidlo, ktoré sa bude vyhľadávať v texte a nájdená zhoda sa bude považovať za chybu.";
        
        private string Correction { get; set; } = string.Empty;
        public string DescriptionCorrection { get; set; } = "Vložte náhradu, ktorou sa nahradí nájdená chyba.";

        private string DescriptionInput { get; set; } = string.Empty;
        public string Description { get; set; } = "Vložte popis chyby.";
        private void OnSaveClick()
        {
            if (!string.IsNullOrEmpty(Rule) && IsValidRegex(Rule))
            {
                var ownRule = new OwnRuleModel
                {
                    RegexRule = Rule,
                    Correction = Correction,
                    Description = DescriptionInput
                };
                SpellingPageDataService.Rules.Add(ownRule);
                NavigationManager.NavigateTo("/spelling");
            }
            else
            {
                ShowErrorMessage();
            }
        }

        private void OnCancelClick()
        {
            NavigationManager.NavigateTo("/spelling");
        }

        private void ShowErrorMessage()
        {
            DescriptionRule = "Pravidlo, ktoré ste zadali nie je platný regex výraz. Skontrolujte ho a skúste znova.";
        }

        private bool IsValidRegex(string pattern)
        {
            try
            {
                Regex.Match("", pattern); // Attempt to create a Regex instance with the pattern
                return true;
            }
            catch (ArgumentException)
            {
                return false; // Invalid regex pattern
            }
        }
    }
}
