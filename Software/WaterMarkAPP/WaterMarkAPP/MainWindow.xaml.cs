using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WaterMarkAPP.Common;
using WaterMarkAPP.Model;
using System.Collections.Generic;
using WaterMarkAPP.Enums;

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

        #region 按钮事件
        /// <summary>
        /// 批文水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFonts_Click(object sender, RoutedEventArgs e)
        {
            DIVWaterMarks(WaterMarkFont());
        }

        /// <summary>
        /// 批图水印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImages_Click(object sender, RoutedEventArgs e)
        {
            DIVWaterMarks(WaterMarkImage());
        }

        /// <summary>
        /// 剪贴板类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBord_Click(object sender, RoutedEventArgs e)
        {
            var list = ClipboardHelper.GetImagePathList();
            if (list == null || list.Count() == 0)
            {
                MessageBox.Show("剪贴板没有图片信息");
            }
            else
            {
                SaveImages(WaterMarkImage(), list.ToArray());
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
            //Environment.Exit(0);
            Application.Current.Shutdown();
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

        #region 配置信息
        //配置信息
        private static Dictionary<string, string> dics = new Dictionary<string, string>();
        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists("Config"))
            {
                Directory.CreateDirectory("Config");
            }
            if (File.Exists("Config/App.dnt"))
            {
                string[] strLines = File.ReadAllLines("Config/App.dnt", System.Text.Encoding.UTF8);
                for (int i = 0; i < strLines.Length; i++)
                {
                    var strs = strLines[i].Split(new char[] { ':', '：' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 2)
                    {
                        dics.Add(strs[0], strs[1]);
                    }
                }
            }
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppSetting.GetAppSetting().ShowDialog();
        }
        #endregion

        #region 软件代码（水印核心代码请看Helper类,最简洁的调用请看APIDemo文件夹里面的内容）

        #region 水印预设
        /// <summary>
        /// 水印文字预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkFont()
        {
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = WaterMarkTypeEnum.Text;
            waterMark.Transparency = 0.7f;
            waterMark.FontStyle = System.Drawing.FontStyle.Bold;
            waterMark.FontFamily = "Consolas";
            waterMark.FontSize = 20f;
            waterMark.BrushesColor = Brushes.YellowGreen;
            waterMark.Text = "dnt.dkill.net";
            waterMark.WaterMarkLocation = WaterMarkLocationEnum.BottomRight;
            if (dics.ContainsKey("Text"))
            {
                waterMark.Text = dics["Text"];
            }
            if (dics.ContainsKey("WaterMarkLocation"))
            {
                WaterMarkLocationEnum resultEnum;
                if (Enum.TryParse(dics["WaterMarkLocation"], out resultEnum))
                {
                    waterMark.WaterMarkLocation = resultEnum;
                }
            }
            return waterMark;
        }

        /// <summary>
        /// 图片水印预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkImage()
        {
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = WaterMarkTypeEnum.Image;
            waterMark.ImgPath = "Config/水印.png";
            waterMark.WaterMarkLocation = WaterMarkLocationEnum.BottomRight;
            waterMark.Transparency = 0.7f;
            if (dics.ContainsKey("ImgPath"))
            {
                waterMark.ImgPath = dics["ImgPath"];
            }
            if (dics.ContainsKey("WaterMarkLocation"))
            {
                WaterMarkLocationEnum resultEnum;
                if (Enum.TryParse(dics["WaterMarkLocation"], out resultEnum))
                {
                    waterMark.WaterMarkLocation = resultEnum;
                }
            }
            return waterMark;
        }
        #endregion

        #region 水印操作

        #region 单个水印操作--暂时不用了
        ///// <summary>
        ///// 单个水印操作
        ///// </summary>
        ///// <param name="waterMark"></param>
        //private static void DIVWaterMark(WaterMark waterMark)
        //{
        //    #region 必须参数获取
        //    OpenFileDialog dialog = new OpenFileDialog
        //    {
        //        Filter = "png(*.png)|*.png|jpg(*.jpg)|*.jpg|bmp(*.bmp)|*.bmp|gif(*.gif)|*.gif|jpeg(*.jpeg)|*.jpeg",
        //        Title = "打开一张图片"
        //    };
        //    if (dialog.ShowDialog() != true)
        //    {
        //        return;
        //    }

        //    //图片路径
        //    string filePath = dialog.FileName;
        //    //文件名
        //    string fileName = Path.GetFileNameWithoutExtension(filePath);
        //    //图片所处目录
        //    string dirPath = Path.GetDirectoryName(filePath);
        //    //存放目录
        //    string savePath = dirPath + "\\DNTWaterMark";
        //    //是否存在，不存在就创建
        //    if (!Directory.Exists(savePath))
        //    {
        //        Directory.CreateDirectory(savePath);
        //    }
        //    #endregion

        //    #region 水印操作
        //    Image successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
        //    if (successImage != null)
        //    {
        //        //保存图片（不管打不打开都保存）
        //        successImage.Save(savePath + "\\" + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);

        //        DisposeImg(successImage);

        //        //是否打开目录
        //        MessageBoxResult result = MessageBox.Show("水印成功！是否打开目录？", "逆天友情提醒", MessageBoxButton.YesNo);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            Process.Start("explorer.exe ", savePath);//打开保存后的路径
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("水印失败！请检查原图和水印图！", "逆天友情提醒");
        //    }
        //    #endregion
        //} 
        #endregion

        /// <summary>
        /// 释放使用资源（避免占用图片）
        /// </summary>
        /// <param name="successImage"></param>
        private static void DisposeImg(Image successImage)
        {
            successImage.Dispose();//释放资源
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
                //类型名进行过滤
                var listFiles = files.Where(f => f.Contains(".png") || f.Contains(".jpg") || f.Contains(".bmp") || f.Contains(".gif") || f.Contains(".jpeg"));
                if (listFiles == null || listFiles.Count() < 1) { MessageBox.Show("该目录木有png，jpg，bmp，gif之类的常用图片格式", "逆天友情提醒"); return; }
                files = listFiles.ToArray();
                SaveImages(waterMark, files);
            }
        }

        /// <summary>
        /// 保存水印后的图片
        /// </summary>
        /// <param name="waterMark"></param>
        /// <param name="filePaths"></param>
        private static void SaveImages(WaterMark waterMark, string[] filePaths)
        {
            int num = 0;
            #region 存储专用
            //图片所处目录
            string dirPath = Path.GetDirectoryName(filePaths[0]);
            //存放目录
            string savePath = string.Format("{0}\\DNTWaterMark", dirPath);
            //是否存在，不存在就创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            #endregion
            foreach (string filePath in filePaths)
            {
                //文件名
                string fileName = Path.GetFileName(filePath);
                var successImage = WaterMarkHelper.SetWaterMark(filePath, waterMark);
                if (successImage != null)
                {
                    //保存图片
                    string imgPath = string.Format(@"{0}\{1}", savePath, fileName);
                    successImage.Save(imgPath, System.Drawing.Imaging.ImageFormat.Png);
                    num++;
                    DisposeImg(successImage);//1.1 释放资源
                }
            }
            //是否打开目录
            MessageBoxResult result = MessageBox.Show("逆天友情提醒：已转换 " + num + " 张图片~是否打开目录？", "转换状态~如果是原图，请检查下水印图片是否有问题", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe ", savePath);//打开保存后的路径
            }
        }
        #endregion

        #endregion        
    }
}