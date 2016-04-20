using System.Windows;

namespace ReadBigFile
{
    /// <summary>
    /// TextMain.xaml 的交互逻辑
    /// </summary>
    public partial class TextMain : Window
    {
        private static TextMain textMain = null;
        private TextMain()
        {
            InitializeComponent();
        }
        public static TextMain GetTextMain()
        {
            if (textMain == null)
            {
                textMain = new TextMain();
            }
            return textMain;
        }
        public void ApendTextLine(string str)
        {
            txt.AppendText(str);
            txt.AppendText("\r\n");
        }
    }
}
