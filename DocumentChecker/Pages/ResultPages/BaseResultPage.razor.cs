using System.Runtime.CompilerServices;

namespace DocumentChecker.Pages.ResultPages
{
    public partial class BaseResultPage
    {
        public virtual string Header { get; set; } = "Výsledok";
        public virtual string TextResult { get; set; } = "Výsledok";

        public virtual async Task OnIgnoreClick()
        {
            TextResult = "Chyba bola ignorovaná";
        }
        public virtual async Task OnCorrectClick()
        {
            TextResult = "Chyba bola opravená";
        }

    }
}
