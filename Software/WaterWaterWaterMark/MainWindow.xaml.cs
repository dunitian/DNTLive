using System;
using System.IO;
using ImageMagick;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace WaterWaterWaterMark
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }

        #region 公用方法
        /// <summary>
        /// 设置水印
        /// </summary>
        /// <param name="imgPaths"></param>
        /// <param name="waterImgPath"></param>
        /// <returns></returns>
        private int SetWaterMark(string[] imgPaths, string waterImgPath)
        {
            int count = 0;
            #region 存储专用
            //图片所处目录
            string dirPath = Path.GetDirectoryName(imgPaths[0]);
            //存放目录
            string savePath = string.Format("{0}\\DNTWaterMark", dirPath);
            //是否存在，不存在就创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            #endregion
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

                            //透明度
                            waterimg.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 2);
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
                    continue;
                }
            }
            return count;
        }
        private void DivWaterMark(string path)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "选择你要批量水印的图片目录"
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = Directory.GetFiles(dialog.SelectedPath);
                //类型名进行过滤
                var listFiles = files.Where(f => f.Contains(".png") || f.Contains(".jpg") || f.Contains(".bmp") || f.Contains(".gif") || f.Contains(".jpeg"));
                if (listFiles == null || listFiles.Count() < 1) { MessageBox.Show("该目录木有png，jpg，bmp，gif之类的常用图片格式", "逆天友情提醒"); return; }
                files = listFiles.ToArray();

                Stopwatch timer = new Stopwatch();
                timer.Start();
                int count = SetWaterMark(files, path);
                timer.Stop();
                MessageBox.Show(string.Format("批量水印了 {0} 张图片！耗时： {1} s", count, timer.ElapsedMilliseconds / 1000));
            }
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
            DivWaterMark(path);
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
            DivWaterMark(path);
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

        #region 联系逆天
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://dnt.dkill.net");
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
