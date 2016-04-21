using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;

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

        string log = "生成失败！";//错误日记
        string charSources = string.Empty; //由哪些组合
        IList<string> dicList = new List<string>();
        /// <summary>
        /// 生成字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileWrite_Click(object sender, RoutedEventArgs e)
        {
            string path = txtPath.Text;
            if (!Directory.Exists(path))
            {
                try { Directory.CreateDirectory(path); }
                catch (System.Exception ex) { ShowLog(ex.Message); return; }
            }
            int num;
            int.TryParse(txtNum.Text, out num);
            if (num <= 0 || num > 999) { ShowLog("字典位数不对头哦！"); return; }
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            if (SaveFile(path, num))
            {
                timer.Stop();
                var result = System.Windows.Forms.MessageBox.Show(string.Format("生成成功！总共：{0}个记录，耗时：{1}", dicList.Count, timer.Elapsed), "是否要打开目录？", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe ", path);//打开保存后的路径
                }
            }
            else
            {
                ShowLog(log);
            }
        }
        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="path"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool SaveFile(string path, int num)
        {
            StringBuilder sb = new StringBuilder();
            bool charB = cboxChar.IsChecked == true ? true : false;
            bool cbox09B = cbox09.IsChecked == true ? true : false;
            bool cboxazB = cboxaz.IsChecked == true ? true : false;
            bool cboxAZB = cboxAZ.IsChecked == true ? true : false;
            if (cbox09B) { sb.Append("0123456789"); }
            if (cboxazB) { sb.Append("abcdefghijklmnopqrstuvwxyz"); }
            if (cboxAZB) { sb.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"); }
            if (charB) { sb.Append("~!@#$%^&*()_-+=\"':;{[]}|\\`?/>.<,*·！￥…（）—、【】“”：；‘’？》《，。"); }
            charSources = sb.ToString();
            GetHackerPass(num);
            try
            {
                System.GC.Collect();
                File.WriteAllLines(string.Format(@"{0}\hacker.txt", path), dicList.ToArray());
            }
            catch (System.Exception ex)
            {
                log = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 得到密码长度从 1到maxLength的所有不同长的密码集合
        /// </summary>
        /// <param name="maxLength"></param>
        public void GetHackerPass(int maxLength)
        {
            for (int i = 1; i <= maxLength; i++)
            {
                char[] chars = new char[i];
                CreateDics(chars, i);
                System.GC.Collect();
            }
        }
        /// <summary>
        /// 得到长度为len所有的密码组合，在字符集charSources中。递归表达式：fn(n)=fn(n-1)*charSources.Length; 
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="len"></param>
        private void CreateDics(char[] chars, int len)
        {
            //递归出口
            if (len == 0)
            {
                dicList.Add(new string(chars));
            }
            else
            {
                for (int i = 0; i < charSources.Length; i++)
                {
                    chars[len - 1] = charSources[i];
                    CreateDics(chars, len - 1);
                }
            }
        }

        #region 弹框信息
        /// <summary>
        /// 弹框
        /// </summary>
        /// <param name="log"></param>
        private static void ShowLog(string log)
        {
            System.Windows.Forms.MessageBox.Show(log, "逆天友情提醒");
        }
        #endregion
        #region 窗体拖动
        /// <summary>
        /// 窗体拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
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
        #region 联系作者
        /// <summary>
        /// 联系作者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void link_click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://dnt.dkill.net");
        }
        #endregion
        #region 设置路径
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPath.Text = Directory.GetCurrentDirectory();
        }
        #endregion
        #region 选择路径
        /// <summary>
        ///存放路径初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog() { Description = "选择你要存放的路径" };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = dialog.SelectedPath;
            }
        }
        #endregion
    }
}
