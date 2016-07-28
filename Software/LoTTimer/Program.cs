using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

/// <summary>
/// By dunitian http://dnt.dkill.net
/// </summary>
namespace LoTTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Init())
            {
                return;
            }
            while (true)
            {
                try
                {
                    Thread.Sleep(new TimeSpan(0, 10, 0));
                    Console.Beep(500, 1500);
                }
                catch
                {
                    //异常还不结束？那岂不是想把系统慢慢拖垮？
                    Console.Beep(3000, 1000);
                    break;
                }
            }
        }

        #region 初始化
        private static bool Init()
        {
            Console.Title = "定时提醒";
            WindowHide("定时提醒");
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                Console.Beep(); Console.Beep(); Console.Beep();//应用已经打开~D N T
                thisProc.Close();
                return false;
            }
            return true;
        }
        #endregion
        #region 隐藏窗口
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        public static void WindowHide(string consoleTitle)
        {
            IntPtr hWnd = FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
                ShowWindow(hWnd, 0);//隐藏窗口
            else
                Console.SetWindowSize(1, 1);
        }
        #endregion
    }
}
