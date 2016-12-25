using System;
using System.IO;
using System.Linq;
using ImageMagick;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Threading.Tasks;

namespace WaterWaterWaterMark
{
    public partial class MainWindow : Window
    {
        double o;//全局变量（透明度，建议1~25）
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

            initConfig();//初始化

        }
        #region 公用方法
        /// <summary>
        /// 设置水印--识别人脸
        /// </summary>
        /// <param name="imgPaths"></param>
        /// <param name="waterImgPath"></param>
        /// <returns></returns>
        private int SetWaterMarkFace(string[] imgPaths, string waterImgPath, string savePath)
        {
            int count = 0;

            for (int k = 0; k < imgPaths.Length; k++)
            {
                //文件名
                string fileName = Path.GetFileName(imgPaths[k]);
                try
                {
                    #region 水印
                    var list = FaceHelper.GetFaceModelList(imgPaths[k]).Result;

                    //原图
                    using (var image = new MagickImage(imgPaths[k]))
                    {
                        int imgWidth = image.Width;
                        int imgHeight = image.Height;

                        #region 单个水印图
                        using (var waterimg = new MagickImage(waterImgPath))
                        {
                            int smallWidth = waterimg.Width;
                            int smallHeight = waterimg.Height;

                            int x = Convert.ToInt32(Math.Ceiling(imgWidth * 1.0 / smallWidth));
                            int y = Convert.ToInt32(Math.Ceiling(imgHeight * 1.0 / smallHeight));

                            //透明度（1~100，越大水印越淡）
                            waterimg.Evaluate(Channels.Alpha, EvaluateOperator.Divide, o);
                            for (int i = 0; i < x; i++)
                            {
                                for (int j = 0; j < y; j++)
                                {
                                    image.Composite(waterimg, i * smallWidth, j * smallHeight, CompositeOperator.Over);//水印
                                }
                            }
                        }
                        #endregion

                        #region 裁剪脸部并还原 2016-12-26
                        //原图
                        using (var magickImg = new MagickImage(imgPaths[k]))
                        {
                            foreach (var item in list)
                            {
                                #region 调整信息
                                if (item.FaceRectangLe == null)
                                    continue;
                                //在原有基础上宽高各*2（PS:我们所谓的不水印脸上其实更多意义在"头"）
                                int addWidth = item.FaceRectangLe.Width / 2;
                                int addHeight = item.FaceRectangLe.Height / 2;
                                //新的宽高
                                item.FaceRectangLe.Width *= 2;
                                item.FaceRectangLe.Height *= 2;
                                //新的左顶点坐标（newX,newY）【PS:不用担心负数问题，Model已经约束了】
                                item.FaceRectangLe.Left -= addWidth;
                                item.FaceRectangLe.Top -= addHeight;
                                #endregion
                                //裁剪
                                magickImg.Crop(item.FaceRectangLe.Left, item.FaceRectangLe.Top, item.FaceRectangLe.Width, item.FaceRectangLe.Height);
                                //还原脸
                                image.Composite(magickImg, item.FaceRectangLe.Left, item.FaceRectangLe.Top, CompositeOperator.Over);
                            }
                        }
                        #endregion

                        image.Write(string.Format(@"{0}\{1}", savePath, fileName));
                        count++;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Images/dnt.log", ex.ToString());
                }
            }
            return count;
        }
        /// <summary>
        /// 设置水印
        /// </summary>
        /// <param name="imgPaths"></param>
        /// <param name="waterImgPath"></param>
        /// <returns></returns>
        private int SetWaterMark(string[] imgPaths, string waterImgPath, string savePath)
        {
            int count = 0;

            for (int k = 0; k < imgPaths.Length; k++)
            {
                //文件名
                string fileName = Path.GetFileName(imgPaths[k]);
                try
                {
                    #region 水印
                    //原图
                    using (var image = new MagickImage(imgPaths[k]))
                    {
                        int imgWidth = image.Width;
                        int imgHeight = image.Height;

                        //单个水印图
                        using (var waterimg = new MagickImage(waterImgPath))
                        {
                            int smallWidth = waterimg.Width;
                            int smallHeight = waterimg.Height;

                            int x = Convert.ToInt32(Math.Ceiling(imgWidth * 1.0 / smallWidth));
                            int y = Convert.ToInt32(Math.Ceiling(imgHeight * 1.0 / smallHeight));

                            //透明度（1~100，越大水印越淡）
                            waterimg.Evaluate(Channels.Alpha, EvaluateOperator.Divide, o);
                            for (int i = 0; i < x; i++)
                            {
                                for (int j = 0; j < y; j++)
                                {
                                    image.Composite(waterimg, i * smallWidth, j * smallHeight, CompositeOperator.Over);//水印
                                }
                            }
                        }
                        image.Write(string.Format(@"{0}\{1}", savePath, fileName));
                        count++;
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    File.AppendAllText("Images/dnt.log", ex.ToString());
                }
            }
            return count;
        }

        /// <summary>
        /// 批量水印
        /// </summary>
        /// <param name="path"></param>
        /// <param name="dicName"></param>
        private void DivWaterMark(string path, string dicName)
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
                var listFiles = files.Where(f => f.ToLower().Contains(".png") || f.ToLower().Contains(".jpg") || f.ToLower().Contains(".bmp") || f.ToLower().Contains(".gif") || f.ToLower().Contains(".jpeg"));
                if (listFiles == null || listFiles.Count() < 1) { MessageBox.Show("该目录木有png，jpg，bmp，gif之类的常用图片格式", "逆天友情提醒"); return; }
                files = listFiles.ToArray();
                #region 存储专用
                //图片所处目录
                string dirPath = Path.GetDirectoryName(files[0]);
                //存放目录
                string savePath = string.Format("{0}\\{1}", dirPath, dicName);
                //是否存在，不存在就创建
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                #endregion
                var result = MessageBox.Show(string.Format("总共识别出 {0} 张图片，操作进行中~~~\n\n是否人脸识别？（请节约使用，每个月使用次数都是有限的）\n\n你可以先去做其他事情，一会再来看看~（人脸识别比较慢）", files.Length), "逆天友情提醒~~~没有人物的就不要人脸识别了！", MessageBoxButton.YesNoCancel);
                //人脸识别
                if (result == MessageBoxResult.Yes)
                {
                    Task.Factory.StartNew(() => SetWaterMarkFace(files, path, savePath));
                }
                else if (result == MessageBoxResult.No)
                {
                    Task.Factory.StartNew(() => SetWaterMark(files, path, savePath));
                }
                if (result != MessageBoxResult.Cancel)
                {
                    Process.Start("explorer.exe ", savePath);//打开保存后的路径
                    //MessageBox.Show($"简化版本不能打开文件夹，请手动打开路径：\n{savePath}", "360误报提醒");
                }
            }
        }
        /// <summary>
        /// 配置文件
        /// </summary>
        private void initConfig()
        {
            if (!File.Exists("Magick.NET-Q8-AnyCPU.dll"))
            {
                MessageBox.Show("Magick.NET-Q8-AnyCPU.dll不存在！", "文件缺失的提示");
                return;
            }

            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }
            //日记专用
            if (!File.Exists("Images/dnt.log"))
            {
                File.Create("Images/dnt.log");
            }
            string configPath = "images/config.dnt";
            if (!File.Exists(configPath))
            {
                File.Create(configPath);
            }
            #region 读取配置文件
            string configStr = File.ReadAllText(configPath);
            if (configStr == null || string.IsNullOrWhiteSpace(configStr))
            {
                o = 4;//默认值
            }
            else
            {
                if (!double.TryParse(configStr, out o))
                {
                    o = 4;//默认值
                }
            }
            #endregion
        }
        #endregion

        #region 按钮事件
        /// <summary>
        /// 白 背 景 水 印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string path = "Images/白色.png";
            if (!File.Exists(path))
            {
                MessageBox.Show("请检查白色水印图是否存在", "Images/白色.png 不存在");
                return;
            }
            DivWaterMark(path, "DNTWhite");
        }

        /// <summary>
        /// 黑 背 景 水 印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string path = "Images/黑色.png";
            if (!File.Exists(path))
            {
                MessageBox.Show("请检查黑色水印图是否存在", "Images/黑色.png 不存在");
                return;
            }
            DivWaterMark(path, "DNTBlack");
        }
        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Setting settingWin = new Setting();
            settingWin.slider.Value = o;
            settingWin.ShowDialog();
            initConfig();
            MessageBox.Show("不透明度配置成功", o.ToString());
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
