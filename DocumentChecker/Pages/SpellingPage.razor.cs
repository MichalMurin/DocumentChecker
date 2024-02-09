namespace DocumentChecker.Pages
{
    public partial class SpellingPage
    {
        public SpellingPage(): base("./Pages/SpellingPage.razor.js")
        {
            
        }
        private void OpenNewRulePage()
        {
            NavigationManager.NavigateTo("/newrule");
        }
        public override void OnStartClick()
        {
            throw new NotImplementedException();
        }
    }
}
