using CommonCode.Interfaces;
using CommonCode.Models;
using CommonCode.ReturnValues;
using CommonCode.Services.DataServices;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using static CommonCode.Formatting.Deffinitions;

namespace DocumentChecker.Pages.ResultPages
{
    public abstract partial class BaseResultPage: ComponentBase
    {
        [Parameter]
        public bool StartScan { get; set; } = false;
        [Inject]
        protected IDataServiceFactory DataServiceFactory { get; set; } = default!;
        protected abstract BaseDataService DataService { get; }
        protected ScanReturnValue? CurrentScan { get; set; }
        public string Header { get; set; } = "Kontrola";
        public string TextResult { get; set; } = "Kontroluje sa dokument...";
        protected virtual string Errorname { get; set; } = "Chyba!";

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
        public virtual async Task OnIgnoreClick()
        {
            if (CurrentScan is not null)
            {
                Console.WriteLine($"Ignoring paragraph: {CurrentScan.ParagraphId}");
                DataService.IgnoredParagraphs.Add(CurrentScan.ParagraphId);
                await HandleIgnoredParagraph();
                SetDisplayedTexts(CheckState.START);
                await ScanDocument();
            }
            else
            {
                SetDisplayedTexts(CheckState.UNEXPECTED_ERROR);
            }
        }
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
        protected virtual async Task HandleIgnoredParagraph()
        {
            await Task.CompletedTask;
        }
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
        protected virtual string GetErrorString(string errorType)
        {
            return "Neznáma chyba";
        }
        protected virtual string GetWarningMessageForError(string errorType)
        {
            if (WarningMessages.ContainsKey(errorType))
            {
                return WarningMessages[errorType];
            }
            return string.Empty;
        }
        protected abstract Task<bool> TryToCorrectParagraph();
        protected abstract Task<ScanReturnValue> GetScanResult(bool isStart);
    }
}
