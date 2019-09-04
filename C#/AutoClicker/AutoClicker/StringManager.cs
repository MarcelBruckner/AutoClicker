using System.Windows.Controls;
using System.Windows.Documents;

namespace AutoClicker
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringManager
    {
        /// <summary>
        /// The return
        /// </summary>
        public const string RETURN = "return";

        /// <summary>
        /// The back
        /// </summary>
        public const string BACK = "back";

        /// <summary>
        /// The tab
        /// </summary>
        public const string TAB = "tab";

        /// <summary>
        /// The space
        /// </summary>
        public const string SPACE = "space";

        /// <summary>
        /// The f keys
        /// </summary>
        public const string F_KEYS = "F";

        /// <summary>
        /// The numbers
        /// </summary>
        public const string NUMBERS = "D";

        /// <summary>
        /// The numpad
        /// </summary>
        public const string NUMPAD = "NumPad";

        /// <summary>
        /// The end loop
        /// </summary>
        public const string END_LOOP = "END LOOP";

        /// <summary>
        /// Converts the rich text box to string.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns></returns>
        public static string ConvertRichTextBoxToString(RichTextBox box)
        {
            return new TextRange(box.Document.ContentStart, box.Document.ContentEnd).Text;
        }

        /// <summary>
        /// Converts the string to block.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns></returns>
        public static Block ConvertStringToBlock(string script)
        {
            return new Paragraph(new Run(script));
        }
    }
}
