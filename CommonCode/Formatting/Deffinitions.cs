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
   
        public enum Alignment
        {
            Centered,
            Justified,
            Left,
            Mixed,
            Right,
            Unknown
        }

        public enum Underline
        {
            Dash,
            DashDotDotHeavy,
            DashDotHeavy,
            DashLong,
            DashLongHeavy,
            DotDash,
            DotDotDash,
            DottedHeavy,
            Double,
            Heavy,
            Mixed,
            None,
            Single,
            Thick,
            Unknown,
            Wave,
            WavyDouble,
            WavyHeavy
        }

        public enum BodyType
        {
            Endnote,
            Footer,
            Footnote,
            Header,
            MainDoc,
            NoteItem,
            Section,
            TableCell,
            Unknown
        }
    }

}
