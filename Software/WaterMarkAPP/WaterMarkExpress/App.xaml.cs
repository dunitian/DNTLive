using System.Diagnostics;
using System.Windows;

namespace WaterMarkAPP
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 单利
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                MessageBox.Show("应用已经打开~", "逆天友情提醒");
                Application.Current.Shutdown();
                return;
            }
            base.OnStartup(e);
        }
    }
}
