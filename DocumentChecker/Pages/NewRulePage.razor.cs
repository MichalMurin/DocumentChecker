using CommonCode.Models;
using CommonCode.Services.DataServices;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace DocumentChecker.Pages
{
    /// <summary>
    /// Represents the page for creating a new rule.
    /// </summary>
    public partial class NewRulePage
    {
        [Inject]
        private SpellingPageDataService SpellingPageDataService { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether an error occurred.
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; } = "Pravidlo, ktoré ste zadali nie je platný regex výraz. Skontrolujte ho a skúste znova.";

        private string Rule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description for the rule.
        /// </summary>
        public string DescriptionRule { get; set; } = "Vložte vlastné RegEx pravidlo, ktoré sa bude vyhľadávať v texte a nájdená zhoda sa bude považovať za chybu.";

        private string Correction { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description for the correction.
        /// </summary>
        public string DescriptionCorrection { get; set; } = "Vložte náhradu, ktorou sa nahradí nájdená chyba.";

        private string DescriptionInput { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description for the input.
        /// </summary>
        public string Description { get; set; } = "Vložte popis chyby.";

        /// <summary>
        /// Handles the click event for the Save button.
        /// </summary>
        private void OnSaveClick()
        {
            IsError = false;
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
                IsError = true;
            }
        }

        /// <summary>
        /// Handles the click event for the Cancel button.
        /// </summary>
        private void OnCancelClick()
        {
            NavigationManager.NavigateTo("/spelling");
        }

        /// <summary>
        /// Checks if the given pattern is a valid regular expression.
        /// </summary>
        /// <param name="pattern">The pattern to check.</param>
        /// <returns><c>true</c> if the pattern is a valid regular expression; otherwise, <c>false</c>.</returns>
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
