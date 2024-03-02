namespace CommonCode.Formatting
{
    public class Deffinitions
    {
        public static Dictionary<string,string> AlignmentDict = new Dictionary<string, string>
        {
            { "Na stred", "Centered" },
            { "Do bloku", "Justified" },
            { "Vľavo", "Left" },
            { "Vpravo", "Right" },  
        };


        public static Dictionary<string, string> FormattingErrors = new Dictionary<string, string>
        {
            { "IncorrectFontName", "Nesprávny font" },
            { "IncorrectFontSize", "Nesprávna veľkosť písma" },
            { "IncorrectAlignment", "Nesprávne zarovnanie textu" },
            { "IncorrectLineSpacing", "Nesprávne riadkovanie" },
            { "IncorrectLeftIndent", "Nesprávny ľavý okraj" },
            { "IncorrectRightIndent", "Nesprávn pravý okraj" }
        };

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
