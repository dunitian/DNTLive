using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WaterMarkAPP.Common
{
    public class ClipboardHelper
    {
        #region 扩展
        /// <summary>
        /// 剪贴板单个文件操作
        /// </summary>
        /// <returns></returns>
        public static Image GetOneImage()
        {
            var data = Clipboard.GetDataObject();
            var formats = data.GetFormats();
            //二进制存储 (存储在剪贴板的截图|画画图内的图片)
            if (data.GetDataPresent(DataFormats.Dib, true))
            {
                var imgSorce = Clipboard.GetImage();
                Bitmap bmp = new Bitmap(imgSorce.PixelWidth, imgSorce.PixelHeight, PixelFormat.Format32bppPArgb);
                BitmapData bmpdata = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                imgSorce.CopyPixels(Int32Rect.Empty, bmpdata.Scan0, bmpdata.Height * bmpdata.Stride, bmpdata.Stride);
                bmp.UnlockBits(bmpdata);
                return bmp as Image;
            }
            //图片文件
            if (data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] objs = (string[])data.GetData(DataFormats.FileDrop, true);
                if (objs != null && objs.Length > 0)
                {
                    return Image.FromFile(objs[0]);
                }
            }
            //剪贴板内单文件
            if (data.GetDataPresent(DataFormats.Bitmap, true))
            {
                return data.GetData(DataFormats.Bitmap, true) as Image;
            }
            //HTML页面里面的图片（网页 + word）
            //暂时不需要支持
            return null;
        }
        #endregion

        public static void SetImage(Image img)
        {
            Clipboard.Clear();
            Clipboard.SetDataObject(img);
        }
        /// <summary>
        /// 获取剪贴板里的图片
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetImagePathList()
        {
            var imgPathList = new List<string>();
            var data = Clipboard.GetDataObject();
            var formats = data.GetFormats();
            //二进制存储 (存储在剪贴板的截图|画画图内的图片)
            if (data.GetDataPresent(DataFormats.Dib, true))
            {
                var imgSorce = Clipboard.GetImage();
                Bitmap bmp = new Bitmap(imgSorce.PixelWidth, imgSorce.PixelHeight, PixelFormat.Format32bppPArgb);
                BitmapData bmpdata = bmp.LockBits(new Rectangle(System.Drawing.Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
                imgSorce.CopyPixels(Int32Rect.Empty, bmpdata.Scan0, bmpdata.Height * bmpdata.Stride, bmpdata.Stride);
                bmp.UnlockBits(bmpdata);
                CreateDirectory();
                string filePath = string.Format(@"Images\{0}.png", Guid.NewGuid());
                bmp.Save(filePath, ImageFormat.Png);
                imgPathList.Add(filePath);
            }
            //图片文件
            if (data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] objs = (string[])data.GetData(DataFormats.FileDrop, true);
                if (objs != null)
                {
                    for (int i = 0; i < objs.Length; i++)
                    {
                        imgPathList.Add(objs[i]);
                    }
                }
            }
            //剪贴板内单文件
            if (data.GetDataPresent(DataFormats.Bitmap, true))
            {
                string filePath = SaveImg(data.GetData(DataFormats.Bitmap, true) as Bitmap);
                if (filePath != null) { imgPathList.Add(filePath); }
            }
            //HTML页面里面的图片（网页 + word）
            if (data.GetDataPresent(DataFormats.Html, true))
            {
                var obj = data.GetData(DataFormats.Html, true);
                if (obj != null)
                {
                    string dataStr = obj.ToString();
                    imgPathList.AddRange(DownloadImg(dataStr));
                }

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
            if (bitmap == null) { return null; }
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
                    string filePath = string.Format(@"Images\{0}", Path.GetFileName(imgPath));
                    webClient.DownloadFile(item.Groups[3].Value, filePath);//剪贴板的图片没有相对路径
                    imgPathList.Add(string.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), filePath));
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
