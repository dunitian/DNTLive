using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using System.Drawing.Imaging;

namespace 剪贴板
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTxt_Click(object sender, EventArgs e)
        {
            var dataStr = Clipboard.GetText();
            if (!string.IsNullOrEmpty(dataStr))
            {
                CreateDirectory("Text");
                string name = string.Format(@"Text\{0}.txt", GetNewName());
                File.WriteAllText(name, dataStr, Encoding.UTF8);
                MessageBox.Show(string.Format("操作成功，请看Text文件夹！", "逆天友情提醒"));
                OpenDirectory("Text");
            }
            else
            {
                MessageBox.Show("剪贴板文本内容为空！", "逆天友情提醒");
            }
        }

        /// <summary>
        /// 生成页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPage_Click(object sender, EventArgs e)
        {
            var dataStr = GetHtmlStr();
            if (!string.IsNullOrEmpty(dataStr))
            {
                MessageBox.Show("操作成功，请看打开的页面！", "逆天友情提醒");
                OutputHtml(dataStr);
            }
            else
            {
                MessageBox.Show("剪贴板图文内容为空！", "逆天友情提醒");
            }
        }

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnWord_Click(object sender, EventArgs e)
        {
            var dataStr = GetHtmlStr();
            if (!string.IsNullOrEmpty(dataStr))
            {
                MessageBox.Show("操作成功，请看打开的页面！", "逆天友情提醒");
                OutputHtml(dataStr, ".doc");
            }
            else
            {
                MessageBox.Show("剪贴板图文内容为空！", "逆天友情提醒");
            }
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImg_Click(object sender, EventArgs e)
        {
            int i = 0;
            var imgObj = Clipboard.GetImage();
            var dataStr = GetHtmlStr();
            int fileCount = GetFileDrop();
            if (imgObj != null)//非HTML的单张图片
            {
                CreateDirectory("Images");
                imgObj.Save(string.Format(@"Images\{0}.png", GetNewName()), ImageFormat.Png);
                MessageBox.Show("操作成功，请看Images文件夹！", "逆天友情提醒");
                OpenDirectory();
            }
            else if (!string.IsNullOrEmpty(dataStr))
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                i = DownloadImg(dataStr);
                watch.Stop();
                MessageBox.Show(string.Format("成功提取{0}个图片,耗时{1}。请查看Images文件夹", i, watch.Elapsed), "逆天友情提醒");
                OpenDirectory();
            }
            else if (fileCount > 0)
            {
                MessageBox.Show(string.Format("成功提取{0}个图片,请查看Images文件夹", fileCount), "逆天友情提醒");
                OpenDirectory();
            }
            else
            {
                MessageBox.Show("剪贴板图片信息为空！", "逆天友情提醒");
            }
        }

        /// <summary>
        /// 本地图片-文件路径
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int GetFileDrop()
        {
            int i = 0;
            var data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] objs = (string[])data.GetData(DataFormats.FileDrop, true);
                if (objs != null)
                {
                    CreateDirectory("Images");
                    for (int j = 0; j < objs.Length; j++)
                    {
                        File.Copy(objs[i], GetNewName());
                        i++;
                    }
                }
            }
            return i;
        }

        /// <summary>
        /// 批量下载图片
        /// </summary>
        /// <param name="dataStr">页面字符串</param>
        /// <param name="i">成功条数</param>
        /// <returns></returns>
        private static int DownloadImg(string dataStr)
        {
            int i = 0;
            var collection = Regex.Matches(dataStr, @"<img([^>]*)\s*src=('|\"")([^'\""]+)('|\"")", RegexOptions.ECMAScript);
            WebClient webClient = new WebClient();
            foreach (Match item in collection)
            {
                string imgPath = item.Groups[3].Value;
                try
                {
                    CreateDirectory("Images");
                    webClient.DownloadFileAsync(new Uri(imgPath), string.Format(@"Images\{0}.png", Path.GetFileName(imgPath)));//剪贴板的图片没有相对路径
                    i++;
                }
                catch (Exception ex) { File.WriteAllText("log.dnt", ex.ToString(), Encoding.UTF8); }
            }
            return i;
        }

        /// <summary>
        /// 清除剪贴板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl1_Click(object sender, EventArgs e)
        {
            ClearClipboard();
            MessageBox.Show("剪贴板清除成功！", "逆天友情提醒");
        }

        #region 公用方法        
        /// <summary>
        /// HTML字符串
        /// </summary>
        /// <returns></returns>
        private static string GetHtmlStr()
        {
            var data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.Html, true))
            {
                return data.GetData(DataFormats.Html, true).ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 输出HTML文件
        /// </summary>
        /// <param name="dataStr"></param>
        /// <param name="ext"></param>
        private static void OutputHtml(string dataStr, string ext = ".html")
        {
            CreateDirectory("Page");
            string name = string.Format(@"Page\{0}{1}", GetNewName(), ext);
            File.WriteAllText(name, dataStr.Substring(dataStr.IndexOf("<html")), Encoding.UTF8);//除去版权信息
            Process.Start(name);
        }

        /// <summary>
        /// 打开目录
        /// </summary>
        private static void OpenDirectory(string typeStr= "images")
        {
            var result = MessageBox.Show("是否打开文件夹？", "逆天提醒", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                Process.Start("explorer.exe ", string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(),typeStr));//打开目录
        }

        /// <summary>
        /// 生成新名称-就不用 Guid 了，普通用户看见了会怕
        /// </summary>
        /// <returns></returns>
        private static string GetNewName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd~HH.mm.ss.fff");
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        private static void CreateDirectory(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
        }

        /// <summary>
        /// 清除剪贴板
        /// </summary>
        private void ClearClipboard()
        {
            Clipboard.Clear();
        }

        #endregion
    }
}
