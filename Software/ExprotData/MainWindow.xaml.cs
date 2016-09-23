using System;
using System.Windows;
using System.Windows.Input;

namespace ExprotData
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region 愚人节过期~（简单版-给别人一条后路）
            DateTime today = DateTime.Now;
            if (DateTime.Compare(today, new DateTime(2017, 04, 01)) > 0)
            {
                MessageBox.Show("愚人节快乐~~软件过期咯~~~\n\n其实逆天故意留了一个小bug，找到你就可以继续用了哦~", "对不起你和逆天不熟，所以逆天的软件不给你用~");
                Application.Current.Shutdown();
            }
            #endregion

        }

        #region 按钮事件
        /// <summary>
        /// 文 件 导 入 咯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 数 据 导 出 呀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 有 新 需 求 了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("请联系逆天：http://dunitian.cnblogs.com/", "新需求提出");
        }
        #endregion

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

        #region 关闭程序
        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
