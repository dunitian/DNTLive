using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace 字典生成器
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

        #region 窗体拖动
        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion
        #region 联系作者
        /// <summary>
        /// 联系作者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_click(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://dnt.dkill.net");
        }
        #endregion
        #region 退出程序
        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        private void FileWrite_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("生成成功！","逆天友情提醒");
        }
    }
}
