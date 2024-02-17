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
        protected void HandleCorrectionResult(bool result)
        {
            if (result)
            {
                TextResult = "Oprava prebehla úspešne";

            }
            else
            {
                TextResult = "Pri oprave nastala chyba, uistite sa, že je odstavec označený!";
            }
        }

    }
}
