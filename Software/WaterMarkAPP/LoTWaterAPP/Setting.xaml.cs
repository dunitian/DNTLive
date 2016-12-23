using System;
using System.IO;
using System.Windows;

namespace WaterWaterWaterMark
{
    /// <summary>
    /// 水印透明度
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                File.WriteAllText("images/config.dnt", slider.Value.ToString("0.00"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入配置文件出错，是不是文件已经打开？请尝试新打开软件~","逆天啰嗦提醒");
                File.AppendAllText("Images/dnt.log", ex.ToString());
            }
        }
    }
}
