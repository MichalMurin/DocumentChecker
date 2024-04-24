using CommonCode.Interfaces;
using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using Microsoft.AspNetCore.Components;
using static CommonCode.Deffinitions.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    /// <summary>
    /// Base class for result pages.
    /// </summary>
    public abstract partial class BaseResultPage : ComponentBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to start the scan.
        /// </summary>
        [Parameter]
        public bool StartScan { get; set; } = false;

        /// <summary>
        /// Gets or sets the data service factory.
        /// </summary>
        [Inject]
        protected IDataServiceFactory DataServiceFactory { get; set; } = default!;

        /// <summary>
        /// Gets the data service.
        /// </summary>
        protected abstract BaseDataService DataService { get; }

        /// <summary>
        /// Gets or sets the current scan result.
        /// </summary>
        protected ScanReturnValue? CurrentScan { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string Header { get; set; } = "Kontrola";

        /// <summary>
        /// Gets or sets the text result.
        /// </summary>
        public string TextResult { get; set; } = "Kontroluje sa dokument...";

        /// <summary>
        /// Gets or sets the error name.
        /// </summary>
        protected virtual string Errorname { get; set; } = "Chyba!";

        /// <summary>
        /// Initializes the component asynchronously.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (StartScan)
            {
                SetDisplayedTexts(CheckState.START);
                Console.WriteLine("Page initialized, starting scan");
                await ScanDocument(true);
            }
        }

        /// <summary>
        /// Handles the click event when the ignore button is clicked.
        /// </summary>
        public virtual async Task OnIgnoreClick()
        {
            if (CurrentScan is not null)
            {
                Console.WriteLine($"Ignoring paragraph: {CurrentScan.ParagraphId}");
                SetDisplayedTexts(CheckState.START);
                DataService.IgnoredParagraphs.Add(CurrentScan.ParagraphId);
                await HandleIgnoredParagraph();
                await ScanDocument();
            }
            else
            {
                SetDisplayedTexts(CheckState.UNEXPECTED_ERROR);
            }
        }

        /// <summary>
        /// Handles the click event when the correct button is clicked.
        /// </summary>
        public virtual async Task OnCorrectClick()
        {
            // perform a correction on a paragraph and run a scan again
            if (CurrentScan is not null)
            {
                Console.WriteLine($"Correcting paragraph: {CurrentScan.ParagraphId}");
                SetDisplayedTexts(CheckState.CORRECTING);
                var correctionResult = await TryToCorrectParagraph();
                if (!correctionResult)
                {
                    SetDisplayedTexts(CheckState.CORRECTION_FAULT);
                }
                else
                {
                    await ScanDocument();
                }
            }
        }

        /// <summary>
        /// Handles the ignored paragraph.
        /// </summary>
        protected virtual async Task HandleIgnoredParagraph()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Scans the document.
        /// </summary>
        /// <param name="start">Indicates whether to start a new scan.</param>
        protected virtual async Task ScanDocument(bool start = false)
        {
            DataService.FoundErrors.Clear();

            CurrentScan = await GetScanResult(start);
            if (CurrentScan.FoundError)
            {
                SetDisplayedTexts(CheckState.FOUND_ERROR);
                FillErrors();
            }
            else
            {
                SetDisplayedTexts(CheckState.FINISHED);
            }
        }

        /// <summary>
        /// Fills the errors found during the scan.
        /// </summary>
        protected virtual void FillErrors()
        {
            if (CurrentScan is not null)
            {
                foreach (var err in CurrentScan.ErrorTypes)
                {
                    DataService.FoundErrors.Add(
                        new FoundErrorModel()
                        {
                            Name = Errorname,
                            Description = GetErrorString(err),
                            ErrorType = err,
                            WarningMessage = GetWarningMessageForError(err)
                        }
                    );
                }
            }
        }

        /// <summary>
        /// Sets the displayed texts based on the check state.
        /// </summary>
        /// <param name="state">The check state.</param>
        protected virtual void SetDisplayedTexts(CheckState state)
        {
            switch (state)
            {
                case CheckState.START:
                    Header = "Kontrola dokumentu";
                    TextResult = "Prebieha kontrola dokumentu, prosím neupravujte dokument počas prebiehajúcej kontroly...";
                    break;
                case CheckState.CORRECTING:
                    Header = "Prebieha oprava";
                    TextResult = $"Prebieha oprava dokumentu. Aby bola oprava úspešná, prosím, nechajte odstavec označený";
                    break;
                case CheckState.CORRECTION_FAULT:
                    Header = "Nastala chyba";
                    TextResult = "Počas opravy nastala chyba, uistite sa, že je označený správny odstavec.";
                    break;
                case CheckState.UNEXPECTED_ERROR:
                    Header = "Nastala chyba";
                    TextResult = "Počas kontroly nastala neožakávaná chyba, začnite kontrolu od znova prosím.";
                    break;
                default:
                    Header = "Kontrola dokumentu";
                    TextResult = "Kontrola dokumentu";
                    break;
            }
        }

        /// <summary>
        /// Gets the error string for the specified error type.
        /// </summary>
        /// <param name="errorType">The error type.</param>
        /// <returns>The error string.</returns>
        protected virtual string GetErrorString(string errorType)
        {
            return "Neznáma chyba";
        }

        /// <summary>
        /// Gets the warning message for the specified error type.
        /// </summary>
        /// <param name="errorType">The error type.</param>
        /// <returns>The warning message.</returns>
        protected virtual string GetWarningMessageForError(string errorType)
        {
            if (WarningMessages.TryGetValue(errorType, out string? value))
            {
                return value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Tries to correct the current paragraph.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected abstract Task<bool> TryToCorrectParagraph();

        /// <summary>
        /// Gets the scan result.
        /// </summary>
        /// <param name="isStart">Indicates whether to start a new scan.</param>
        /// <returns>The scan result.</returns>
        protected abstract Task<ScanReturnValue> GetScanResult(bool isStart);
    }
}
