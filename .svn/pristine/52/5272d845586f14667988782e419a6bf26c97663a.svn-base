using System.Windows;
using System.IO;
using System.Text;

namespace WaterMarkAPP
{
    /// <summary>
    /// AppSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AppSetting : Window
    {
        private AppSetting()
        {
            InitializeComponent();
        }

        private static AppSetting appSetting = null;
        /// <summary>
        /// 单利
        /// </summary>
        /// <returns></returns>
        public static AppSetting GetAppSetting()
        {
            if (appSetting == null) { return new AppSetting(); }
            return appSetting;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("Config"))
            {
                Directory.CreateDirectory("Config");
            }
            string fontStr = txtFontStr.Text;
            string imgPath = txtImgPath.Text;
            var obj = comboLocation.SelectedItem as System.Windows.Controls.ComboBoxItem;
            int locationIndex = 9;
            switch (obj.Content.ToString())
            {
                case "顶部居左":
                    locationIndex = 1;
                    break;
                case "顶部居中":
                    locationIndex = 2;
                    break;
                case "顶部居右":
                    locationIndex = 3;
                    break;
                case "中部居左":
                    locationIndex = 4;
                    break;
                case "中部居中":
                    locationIndex = 5;
                    break;
                case "中部居右":
                    locationIndex = 6;
                    break;
                case "底部居左":
                    locationIndex = 7;
                    break;
                case "底部居中":
                    locationIndex = 8;
                    break;
                case "底部居右":
                    locationIndex = 9;
                    break;
            }
            try
            {
                File.WriteAllText("Config/App.dnt", string.Format("Text：{0}\r\nImgPath：{1}\r\nWaterMarkLocation：{2}", fontStr, imgPath, locationIndex), Encoding.UTF8);
            }
            catch (System.Exception ex)
            {
                File.WriteAllText("Config/log.dnt", ex.ToString(), Encoding.UTF8);
            }
            MessageBox.Show("配置成功,请重新打开软件！", "逆天友情提醒");
            Application.Current.Shutdown(-1);
        }
    }
}
