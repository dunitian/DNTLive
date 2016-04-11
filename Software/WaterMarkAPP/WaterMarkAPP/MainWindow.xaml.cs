using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaterMarkAPP.Common;
using WaterMarkAPP.Model;

namespace WaterMarkAPP
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

        /// <summary>
        /// 单文水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFont_Click(object sender, RoutedEventArgs e)
        {
            WaterMark waterMark = WaterMarkFont();
            DIVWaterMark(waterMark);
        }

        /// <summary>
        /// 批文水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFonts_Click(object sender, RoutedEventArgs e)
        {
            WaterMark waterMark = WaterMarkFont();
            DIVWaterMarks(waterMark);
        }

        /// <summary>
        /// 单图水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            WaterMark waterMark = WaterMarkImage();
            DIVWaterMark(waterMark);
        }

        /// <summary>
        /// 批图水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImages_Click(object sender, RoutedEventArgs e)
        {
            WaterMark waterMark = WaterMarkImage();
            DIVWaterMarks(waterMark);
        }

        #region 关闭程序
        /// <summary>
        /// 关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        #region 版权系列
        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Process.Start("http://dunitian.cnblogs.com/");
        }

        private void TextBlock_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Process.Start("http://tieba.baidu.com/f?kw=毒逆天");
        }

        private void TextBlock_MouseLeave_2(object sender, MouseEventArgs e)
        {
            Process.Start("http://1054186320.qzone.qq.com/");
        }
        #endregion

        #region WPF核心代码（水印核心代码请看Helper类）

        #region 水印预设
        /// <summary>
        /// 水印文字预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkFont()
        {
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = Enums.WaterMarkTypeEnum.Text;
            waterMark.Transparency = 0.7f;
            waterMark.Text = "dunitian.cnblogs.com";
            waterMark.FontStyle = System.Drawing.FontStyle.Bold;
            waterMark.FontFamily = "Consolas";
            waterMark.FontSize = 20f;
            waterMark.BrushesColor = System.Drawing.Brushes.Black;
            waterMark.WaterMarkLocation = Enums.WaterMarkLocationEnum.CenterCenter;
            return waterMark;
        }

        /// <summary>
        /// 图片水印预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkImage()
        {
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = Enums.WaterMarkTypeEnum.Image;
            waterMark.ImgPath = "水印.png";
            waterMark.WaterMarkLocation = Enums.WaterMarkLocationEnum.BottomRight;
            waterMark.Transparency = 0.7f;
            return waterMark;
        }
        #endregion

        #region 水印操作
        /// <summary>
        /// 单个水印操作
        /// </summary>
        /// <param name="waterMark"></param>
        private static void DIVWaterMark(WaterMark waterMark)
        {
            #region 必须参数获取
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "png(*.png)|*.png|jpg(*.jpg)|*.jpg|bmp(*.bmp)|*.bmp|gif(*.gif)|*.gif|jpeg(*.jpeg)|*.jpeg",
                Title = "打开一张图片"
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            //图片路径
            string filePath = dialog.FileName;
            //文件名
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            //图片所处目录
            string dirPath = System.IO.Path.GetDirectoryName(filePath);
            //存放目录
            string savePath = dirPath + "\\DNTWaterMark";
            //是否存在，不存在就创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            #endregion

            #region 水印操作
            Image successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
            if (successImage != null)
            {
                //保存图片（不管打不打开都保存）
                successImage.Save(savePath + "\\" + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                //是否打开目录
                MessageBoxResult result = MessageBox.Show("水印成功！是否打开目录？", "逆天友情提醒", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe ", savePath);//打开保存后的路径
                }
            }
            else
            {
                MessageBox.Show("水印失败！请检查原图和水印图！", "逆天友情提醒");
            }
            #endregion
        }

        /// <summary>
        /// 批量水印操作
        /// </summary>
        /// <param name="waterMark"></param>
        private void DIVWaterMarks(WaterMark waterMark)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择你要批量水印的图片目录"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dialog.SelectedPath);
                if (files.Length <= 0)
                {
                    return;
                }

                #region 存储专用
                //图片所处目录
                string dirPath = System.IO.Path.GetDirectoryName(files[0]);
                //存放目录
                string savePath = dirPath + "\\DNTWaterMark";
                //是否存在，不存在就创建
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                #endregion

                int num = 0;//计数用
                foreach (string filePath in files)
                {
                    //文件名
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                    Image successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
                    if (successImage != null)
                    {
                        //保存图片
                        successImage.Save(savePath + "\\" + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        num++;
                    }
                }

                //是否打开目录
                MessageBoxResult result = MessageBox.Show("逆天友情提醒：已转换 " + num + " 张图片~是否打开目录？", "转换状态", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe ", savePath);//打开保存后的路径
                }
            }
        }
        #endregion

        #endregion
    }
}
