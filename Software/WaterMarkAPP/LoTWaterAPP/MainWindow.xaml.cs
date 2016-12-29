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

        #region 人脸识别-水印
        /// <summary>
        /// 设置水印--识别人脸
        /// </summary>
        /// <param name="imgPaths">原图路径</param>
        /// <param name="waterImgPath">水印图路径</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="thumPath">水印路径</param>
        /// <param name="facePath">人脸路径</param>
        /// <returns></returns>
        private void SetWaterMarkFace(string[] imgPaths, string waterImgPath, string savePath, string thumPath, string facePath)
        {
            for (int k = 0; k < imgPaths.Length; k++)
            {
                string imgPath = imgPaths[k];
                //文件名
                string fileName = Path.GetFileName(imgPath);

                #region 生成缩略图
                //缩略图图片路径
                string thumImagePath = $"{thumPath}\\{fileName}";
                //为了提高网络传输速度：V2会先生成缩略图再上传
                using (var smallImg = new MagickImage(imgPath))
                {
                    smallImg.Resize(smallImg.Width / 2, smallImg.Height / 2);
                    smallImg.Write(thumImagePath);
                }
                #endregion

                //获取脸部信息
                var task = FaceHelper.GetFaceModelList(thumImagePath);//Task.Factory.StartNew(() => FaceHelper.GetFaceModelList(thumImagePath));
                var list = task.Result;

                #region 水印
                //原图
                using (var image = new MagickImage(imgPath))
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
                    //识别不出来脸就不还原
                    if (list != null && list.Count() > 0)
                    {
                        //原图 （大小*2）
                        foreach (var item in list)
                        {
                            using (var magickImg = new MagickImage(imgPath))
                            {
                                #region 调整信息
                                if (item.FaceRectangLe == null)
                                    continue;
                                //尺寸还原
                                int width = item.FaceRectangLe.Width * 2;
                                int height = item.FaceRectangLe.Height * 2;
                                int x = item.FaceRectangLe.Left * 2;
                                int y = item.FaceRectangLe.Top * 2;

                                //在原有基础上宽高各*2（PS:我们所谓的不水印脸上其实更多意义在"头"）
                                //新的宽高
                                width *= 2;
                                height *= 2;
                                //新的左顶点坐标（newX,newY）【PS:不用担心负数问题，Model已经约束了】
                                x -= item.FaceRectangLe.Width;
                                y -= item.FaceRectangLe.Height;
                                #endregion

                                //裁剪
                                magickImg.Crop(x, y, width, height);
                                //保存一下脸（到时候可以查看误差）[一张图可能多个脸]
                                magickImg.Write(string.Format(@"{0}\{1}{2}", facePath, item.FaceId, fileName));
                                //还原脸（Image是水印图的Magick对象）
                                image.Composite(magickImg, x, y, CompositeOperator.Over);
                            }
                        }
                    }
                    #endregion

                    image.Write(string.Format(@"{0}\{1}", savePath, fileName));
                }
                #endregion

            }
        }
        #endregion

        #region 普通方式-水印
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
                    lock (this)
                    {
                        File.AppendAllText("Images/dnt.log", ex.ToString());
                    }
                }
            }
            return count;
        }
        #endregion

        #region 批量水印
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
                    #region 是否存在，不存在就创建（缩略图，脸）
                    string thumPath = $"{savePath}\\thum";
                    if (!Directory.Exists(thumPath))
                    {
                        Directory.CreateDirectory(thumPath);
                    }
                    string facePath = $"{savePath}\\face";
                    if (!Directory.Exists(facePath))
                    {
                        Directory.CreateDirectory(facePath);
                    }
                    #endregion

                    var task = Task.Run(() => SetWaterMarkFace(files, path, savePath, thumPath, facePath));
                    task.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            var ex = t.Exception.GetBaseException();
                            MessageBox.Show(ex.Message);
                            lock (this)
                            {
                                File.AppendAllText("Images/dnt.log", ex.ToString());
                            }
                        }
                        if (t.IsCompleted)
                        {
                            MessageBox.Show("thum目录是缩略图，可以直接删了\n\nface目录是识别出来的人脸，可以看看再删", "我已经完成了，您老看看呗~");
                        }
                    });
                }
                else if (result == MessageBoxResult.No)
                {
                    Task.Run(() => SetWaterMark(files, path, savePath));
                }
                if (result != MessageBoxResult.Cancel)
                {
                    Process.Start("explorer.exe ", savePath);//打开保存后的路径
                    //MessageBox.Show($"简化版本不能打开文件夹，请手动打开路径：\n{savePath}", "360误报提醒");
                }
            }
        }
        #endregion

        #region 配置文件
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
