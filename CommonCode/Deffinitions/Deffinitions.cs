namespace CommonCode.Deffinitions
{
    /// <summary>
    /// Class containing definitions for various constants and dictionaries.
    /// </summary>
    public class Deffinitions
    {
        /// <summary>
        /// Dictionary mapping alignment options in the source language to alignment options in the target language.
        /// </summary>
        public static Dictionary<string, string> AlignmentDict = new Dictionary<string, string>
            {
                { "Na stred", "Centered" },
                { "Do bloku", "Justified" },
                { "Vľavo", "Left" },
                { "Vpravo", "Right" },
            };

        /// <summary>
        /// Dictionary mapping formatting error codes to their corresponding error messages.
        /// </summary>
        public static Dictionary<string, string> FormattingErrors = new Dictionary<string, string>
            {
                { "IncorrectFontName", "Nesprávny font" },
                { "IncorrectFontSize", "Nesprávna veľkosť písma" },
                { "IncorrectAlignment", "Nesprávne zarovnanie textu" },
                { "IncorrectLineSpacing", "Nesprávne riadkovanie" },
                { "IncorrectLeftIndent", "Nesprávny ľavý okraj" },
                { "IncorrectRightIndent", "Nesprávn pravý okraj" }
            };

        /// <summary>
        /// Dictionary mapping consistency error codes to their corresponding error messages.
        /// </summary>
        public static Dictionary<string, string> ConsistencyErrors = new Dictionary<string, string>
            {
                { "DoubleSpaces", "Dvojité medzery" },
                { "EmptyLines", "Prázdne riadky" },
                { "InvalidCrossRef", "Neplatný krížový odkaz" },
                { "InvalidHeadingContinuity", "Neplatná kontinuita nadpisu" },
                { "InvalidHeadingConsistency", "Neplatná konzistencia nadpisu" },
                { "InvalidHeadingNumberConsistency", "Neplatná konzistencia čísla nadpisu" },
                { "InconsistentFormatting", "Nejednotné formátovanie" },
                { "InvalidParenthesis", "Neplatné zátvorky" },
                { "InvalidDotsComasColons", "Neplatné bodky, čiarky, dvojbodky" },
                { "CaptionMissing", "Chýbajúci popis" },
                { "InvalidListConsistency", "Neplatná konzistencia zoznamu" }
            };

        /// <summary>
        /// Dictionary mapping warning codes to their corresponding warning messages.
        /// </summary>
        public static Dictionary<string, string> WarningMessages = new Dictionary<string, string>
            {
                { "InvalidHeadingContinuity", "Chyba sa nedá opraviť automaticky!" },
                { "InvalidDotsComasColons", "Pri oprave chyby sa pridajú medzery za bodky v texte, takže sa môže nesprávne upraviť niektorý text ako URL adresa!" },
                { "InvalidListConsistency", "Chyba sa nedá opraviť automaticky!" },
                { "CaptionMissing", "Chyba sa nedá opraviť automaticky!" },
                { "InvalidParenthesis", "Chyba sa nedá opraviť automaticky!" },
            };

        /// <summary>
        /// Enumeration representing the different states of the checking process.
        /// </summary>
        public enum CheckState
        {
            START,
            FOUND_ERROR,
            CORRECTING,
            FINISHED,
            CORRECTION_FAULT,
            UNEXPECTED_ERROR
        }
    }

}
