using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ReadBigFile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadBigFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"L:\NET之路\逆天SQL专题\3.大数据专题\BigValues.sql";
            if (!File.Exists(filePath)) { return; }
            using (var stream = File.OpenRead(filePath))
            {
                //var encoding = EncodingHelper.GetType(stream);
                var textMain = TextMain.GetTextMain();
                textMain.Show();
                byte[] buffer = new byte[1024 * 1024 * 10];
                while (true)
                {
                    if (!stream.CanRead) { break; }
                    int r = stream.Read(buffer, 0, buffer.Length);
                    textMain.ApendTextLine(Encoding.UTF8.GetString(buffer, 0, buffer.Length));
                    if (r == 0) { break; }
                }
            }
        }

        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SayByBy_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
