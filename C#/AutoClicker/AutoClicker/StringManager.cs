using System.Windows.Controls;
using System.Windows.Documents;

namespace AutoClicker
{
    public static class StringManager
    {
        public const string RETURN = "return";
        public const string BACK = "back";
        public const string TAB = "tab";
        public const string SPACE = "space";

        public const string F_KEYS = "F";
        public const string NUMBERS = "D";
        public const string NUMPAD = "NumPad";

        public const string END_LOOP = "END LOOP";

        public static string RichTextBoxToString(RichTextBox box)
        {
            return new TextRange(box.Document.ContentStart, box.Document.ContentEnd).Text;
        }
    }
}
