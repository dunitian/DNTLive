using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace WaterMarkAPP.Common
{
    public class ClipboardHelper
    {
        /// <summary>
        /// 获取剪贴板里的图片
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetImagePathList()
        {
            var imgPathList = new List<string>();
            var data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.Bitmap, true))
            {
                string filePath = SaveImg(data.GetData(DataFormats.Bitmap, true) as Bitmap);
                if (filePath != null) { imgPathList.Add(filePath); }
            }
            if (data.GetDataPresent(DataFormats.Html, true))
            {
                string dataStr = data.GetData(DataFormats.Html, true).ToString();
                imgPathList.AddRange(DownloadImg(dataStr));
            }
            return imgPathList;
        }

        /// <summary>
        /// 保存图片，返回图片地址
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static string SaveImg(Bitmap bitmap)
        {
            CreateDirectory();
            string filePath = string.Format(@"Images\{0}.png", Guid.NewGuid());
            try { bitmap.Save(filePath, ImageFormat.Png); return filePath; }
            catch (Exception ex) { DNTLog(ex); return null; }
        }

        /// <summary>
        /// 批量下载图片
        /// </summary>
        /// <param name="dataStr">页面字符串</param>
        /// <param name="i">成功条数</param>
        /// <returns></returns>
        private static IEnumerable<string> DownloadImg(string dataStr)
        {
            var imgPathList = new List<string>();
            var collection = Regex.Matches(dataStr, @"<img([^>]*)\s*src=('|\"")([^'\""]+)('|\"")", RegexOptions.ECMAScript);
            WebClient webClient = new WebClient();
            foreach (Match item in collection)
            {
                string imgPath = item.Groups[3].Value;
                try
                {
                    CreateDirectory();
                    string filePath = string.Format(@"Images\{0}.png", Path.GetFileName(imgPath));
                    webClient.DownloadFileAsync(new Uri(imgPath), filePath);//剪贴板的图片没有相对路径
                    imgPathList.Add(string.Format("{0}\filePath", Directory.GetCurrentDirectory()));
                }
                catch (Exception ex) { DNTLog(ex); }
            }
            return imgPathList;
        }

        private static void DNTLog(Exception ex)
        {
            File.WriteAllText("log.dnt", ex.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        private static void CreateDirectory()
        {
            if (!Directory.Exists("Images"))
            {
                Directory.CreateDirectory("Images");
            }
        }
    }
}
