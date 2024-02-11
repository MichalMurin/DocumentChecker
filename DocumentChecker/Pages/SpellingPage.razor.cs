namespace DocumentChecker.Pages
{
    public partial class SpellingPage
    {
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
