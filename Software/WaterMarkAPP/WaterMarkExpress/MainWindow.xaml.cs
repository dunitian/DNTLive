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
            //检查一下水印图片是否存在
            if (dics.ContainsKey("ImgPath"))
            {
                if (!File.Exists(dics["ImgPath"]))
                {
                    MessageBox.Show(string.Format("请检查水印图片 {0} 是否存在!", dics["ImgPath"]), "逆天友情提醒");
                    Application.Current.Shutdown();
                }
            }
        }       
        #endregion

        #region 按钮事件
        /// <summary>
        /// 水印后依旧放剪贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFonts_Click(object sender, RoutedEventArgs e)
        {
            var model = WaterMarkImage();
            var img = ClipboardHelper.GetOneImage();
            if (img == null)
            {
                MessageBox.Show("剪贴板没有图片信息");
            }
            else
            {
                ClipboardHelper.SetImage(WaterMarkHelper.SetWaterMark(img, model));
            }
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

        #region 软件代码

        #region 水印预设
        /// <summary>
        /// 图片水印预设
        /// </summary>
        /// <returns></returns>
        private static WaterMark WaterMarkImage()
        {
            WaterMark waterMark = new WaterMark();
            waterMark.WaterMarkType = WaterMarkTypeEnum.Image;
            waterMark.Transparency = 0.7f;
            //图片路径
            if (dics.ContainsKey("ImgPath"))
            {
                waterMark.ImgPath = dics["ImgPath"];
            }
            //水印位置
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