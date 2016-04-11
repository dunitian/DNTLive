using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WaterMarkAPP.Common;
using WaterMarkAPP.Model;

namespace WaterMarkAPP
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class WaterMarkAPI : Window
    {
        public WaterMarkAPI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文字水印调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //给水印对象赋对应的值
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = Enums.WaterMarkTypeEnum.Text;
            waterMark.Transparency = 0.7f;
            waterMark.Text = "dunitian.cnblogs.com";
            waterMark.FontStyle = System.Drawing.FontStyle.Bold;
            waterMark.FontFamily = "Consolas";
            waterMark.FontSize = 20f;
            waterMark.BrushesColor = System.Drawing.Brushes.YellowGreen;
            waterMark.WaterMarkLocation = Enums.WaterMarkLocationEnum.CenterCenter;

            //调用
            Image successImage = WaterMarkHelper.SetWaterMark("text.png", waterMark);
            //保存
            successImage.Save("text1.png", System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show("请查看软件根目录", "成功");
        }

        /// <summary>
        /// 图片水印调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //给水印对象赋对应的值
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = Enums.WaterMarkTypeEnum.Image;
            waterMark.ImgPath = "水印.png";
            waterMark.WaterMarkLocation = Enums.WaterMarkLocationEnum.CenterCenter;
            waterMark.Transparency = 0.7f;

            //调用
            Image successImage = WaterMarkHelper.SetWaterMark("text.png", waterMark);
            //保存
            successImage.Save("text2.png", System.Drawing.Imaging.ImageFormat.Png);

            MessageBox.Show("请查看软件根目录","成功");
        }
    }
}
