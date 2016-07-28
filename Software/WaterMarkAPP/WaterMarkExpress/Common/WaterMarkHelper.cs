using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using WaterMarkAPP.Enums;
using WaterMarkAPP.Model;

namespace WaterMarkAPP.Common
{

    /// <summary>
    /// 水印帮助类
    /// </summary>
    public class WaterMarkHelper
    {
        public static bool TryFromFile(string imgPath, ref Image img)
        {
            try
            {
                img = Image.FromFile(imgPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region 设置水印
        /// <summary>
        /// 设置水印
        /// </summary>
        /// <param name="imgPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Image SetWaterMark(string imgPath, WaterMark model)
        {
            Image imgSource = null;//背景图
            Image markImg = null;//水印图片
            if (!TryFromFile(imgPath, ref imgSource))
            {
                return null;
            }

            //水印检验（文字，图片[路径下是否存在图片]）
            #region 水印校验+水印处理
            if (model == null) { return null; }
            if (!System.IO.File.Exists(imgPath)) { return null; }//看看原图是否存在
            //根据水印类型校验+水印处理
            switch (model.WaterMarkType)
            {
                case WaterMarkTypeEnum.Text:
                    if (string.IsNullOrEmpty(model.Text))
                    {
                        return null;
                    }
                    else
                    {
                        markImg = TextToImager(model);//水印处理-如果是文字就转换成图片
                    }
                    break;
                case WaterMarkTypeEnum.Image:
                    if (!System.IO.File.Exists(model.ImgPath))
                    {
                        return null;
                    }
                    else
                    {
                        if (!TryFromFile(model.ImgPath, ref markImg))//获得水印图像
                        {
                            return imgSource;
                        }
                    }
                    break;
                case WaterMarkTypeEnum.NoneMark:
                    return imgSource;
            }
            #endregion

            #region 创建颜色矩阵
            //创建颜色矩阵
            float[][] ptsArray ={
                                 new float[] {1, 0, 0, 0, 0},
                                 new float[] {0, 1, 0, 0, 0},
                                 new float[] {0, 0, 1, 0, 0},
                                 new float[] {0, 0, 0, model.Transparency, 0}, //注意：0.0f为完全透明，1.0f为完全不透明
                                 new float[] {0, 0, 0, 0, 1}};
            ColorMatrix colorMatrix = new ColorMatrix(ptsArray);
            //新建一个Image属性
            ImageAttributes imageAttributes = new ImageAttributes();
            //将颜色矩阵添加到属性
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            #endregion

            //原图格式检验+水印
            #region 原图格式检验+水印
            //判断是否是索引图像格式
            if (imgSource.PixelFormat == PixelFormat.Format1bppIndexed || imgSource.PixelFormat == PixelFormat.Format4bppIndexed || imgSource.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                #region 索引图片,转成位图再加图片
                //转成位图,这步很重要 
                Bitmap bitmap = new Bitmap(imgSource.Width, imgSource.Height);
                Graphics graphic = Graphics.FromImage(bitmap);

                #region 缩放处理
                //如果原图小于水印图片 等比缩放水印图
                if (markImg.Width >= imgSource.Width || markImg.Height >= imgSource.Height)
                {
                    markImg = ImageShrink(imgSource, markImg);
                }
                #endregion

                #region 水印位置
                //水印位置
                int x;
                int y;
                WaterMarkLocations(model, imgSource, markImg, out x, out y);
                #endregion

                //将原图画在位图上
                graphic.DrawImage(imgSource, new Point(0, 0));

                //将水印加在位图上
                graphic.DrawImage(markImg, new Rectangle(x, y, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);

                graphic.Dispose();
                return bitmap;
                #endregion
            }
            else
            {
                #region 非索引图片,直接在上面加上水印
                Graphics graphic = Graphics.FromImage(imgSource);

                #region 缩放处理
                //如果原图小于水印图片 等比缩放水印图
                if (markImg.Width >= imgSource.Width || markImg.Height >= imgSource.Height)
                {
                    markImg = ImageShrink(imgSource, markImg);
                }
                #endregion

                #region 水印位置
                //水印位置
                int x;
                int y;
                WaterMarkLocations(model, imgSource, markImg, out x, out y);
                #endregion

                //将水印加在原图上
                graphic.DrawImage(markImg, new Rectangle(x, y, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);

                graphic.Dispose();
                return imgSource;
                #endregion
            }
            #endregion
        }

        #region 扩展
        /// <summary>
        /// 设置水印-方法重载
        /// </summary>
        /// <param name="imgSource">背景图</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Image SetWaterMark(Image imgSource, WaterMark model)
        {
            Image markImg = null;//水印图片

            #region 水印校验+水印处理
            //水印检验
            if (model == null || imgSource == null) { return null; }

            //根据水印类型校验+水印处理
            switch (model.WaterMarkType)
            {
                case WaterMarkTypeEnum.Text:
                    if (string.IsNullOrEmpty(model.Text))
                    {
                        return null;
                    }
                    else
                    {
                        markImg = TextToImager(model);//水印处理-如果是文字就转换成图片
                    }
                    break;
                case WaterMarkTypeEnum.Image:
                    if (!System.IO.File.Exists(model.ImgPath))
                    {
                        return null;
                    }
                    else
                    {
                        if (!TryFromFile(model.ImgPath, ref markImg))//获得水印图像
                        {
                            return imgSource;
                        }
                    }
                    break;
                case WaterMarkTypeEnum.NoneMark:
                    return imgSource;
            }
            #endregion

            #region 创建颜色矩阵
            //创建颜色矩阵
            float[][] ptsArray ={
                                 new float[] {1, 0, 0, 0, 0},
                                 new float[] {0, 1, 0, 0, 0},
                                 new float[] {0, 0, 1, 0, 0},
                                 new float[] {0, 0, 0, model.Transparency, 0}, //注意：0.0f为完全透明，1.0f为完全不透明
                                 new float[] {0, 0, 0, 0, 1}};
            ColorMatrix colorMatrix = new ColorMatrix(ptsArray);
            //新建一个Image属性
            ImageAttributes imageAttributes = new ImageAttributes();
            //将颜色矩阵添加到属性
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
            #endregion

            //原图格式检验+水印
            #region 原图格式检验+水印
            //判断是否是索引图像格式
            if (imgSource.PixelFormat == PixelFormat.Format1bppIndexed || imgSource.PixelFormat == PixelFormat.Format4bppIndexed || imgSource.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                #region 索引图片,转成位图再加图片
                //转成位图,这步很重要 
                Bitmap bitmap = new Bitmap(imgSource.Width, imgSource.Height);
                Graphics graphic = Graphics.FromImage(bitmap);

                #region 缩放处理
                //如果原图小于水印图片 等比缩放水印图
                if (markImg.Width >= imgSource.Width || markImg.Height >= imgSource.Height)
                {
                    markImg = ImageShrink(imgSource, markImg);
                }
                #endregion

                #region 水印位置
                //水印位置
                int x;
                int y;
                WaterMarkLocations(model, imgSource, markImg, out x, out y);
                #endregion

                //将原图画在位图上
                graphic.DrawImage(imgSource, new Point(0, 0));

                //将水印加在位图上
                graphic.DrawImage(markImg, new Rectangle(x, y, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);

                graphic.Dispose();
                return bitmap;
                #endregion
            }
            else
            {
                #region 非索引图片,直接在上面加上水印
                Graphics graphic = Graphics.FromImage(imgSource);

                #region 缩放处理
                //如果原图小于水印图片 等比缩放水印图
                if (markImg.Width >= imgSource.Width || markImg.Height >= imgSource.Height)
                {
                    markImg = ImageShrink(imgSource, markImg);
                }
                #endregion

                #region 水印位置
                //水印位置
                int x;
                int y;
                WaterMarkLocations(model, imgSource, markImg, out x, out y);
                #endregion

                //将水印加在原图上
                graphic.DrawImage(markImg, new Rectangle(x, y, markImg.Width, markImg.Height), 0, 0, markImg.Width, markImg.Height, GraphicsUnit.Pixel, imageAttributes);

                graphic.Dispose();
                return imgSource;
                #endregion
            }
            #endregion
        }
        #endregion
        #endregion

        #region 水印处理-文字转图片
        /// <summary>
        /// 水印处理-文字转图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Image TextToImager(WaterMark model)
        {
            Font f = new Font(model.FontFamily, model.FontSize, model.FontStyle);
            Bitmap fbitmap = new Bitmap(Encoding.GetEncoding("GBK").GetByteCount(model.Text) / 2 * f.Height, f.Height);
            Graphics gh = Graphics.FromImage(fbitmap);//创建一个画板;
            gh.SmoothingMode = SmoothingMode.AntiAlias;
            gh.DrawString(model.Text, f, model.BrushesColor, 0, 0);//画字符串
            return fbitmap as Image;
        }
        #endregion

        #region 水印位置
        /// <summary>
        /// 水印位置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="imgSource"></param>
        /// <param name="markImg"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void WaterMarkLocations(WaterMark model, Image imgSource, Image markImg, out int x, out int y)
        {
            x = 0;
            y = 0;
            switch (model.WaterMarkLocation)
            {
                case WaterMarkLocationEnum.TopLeft:
                    x = 0;
                    y = 0;
                    break;
                case WaterMarkLocationEnum.TopCenter:
                    x = imgSource.Width / 2 - markImg.Width / 2;
                    y = 0;
                    break;
                case WaterMarkLocationEnum.TopRight:
                    x = imgSource.Width - markImg.Width;
                    y = 0;
                    break;
                case WaterMarkLocationEnum.CenterLeft:
                    x = 0;
                    y = imgSource.Height / 2 - markImg.Height / 2;
                    break;
                case WaterMarkLocationEnum.CenterCenter:
                    x = imgSource.Width / 2 - markImg.Width / 2;
                    y = imgSource.Height / 2 - markImg.Height / 2;
                    break;
                case WaterMarkLocationEnum.CenterRight:
                    x = imgSource.Width - markImg.Width;
                    y = imgSource.Height / 2 - markImg.Height / 2;
                    break;
                case WaterMarkLocationEnum.BottomLeft:
                    x = 0;
                    y = imgSource.Height - markImg.Height;
                    break;
                case WaterMarkLocationEnum.BottomCenter:
                    x = imgSource.Width / 2 - markImg.Width / 2;
                    y = imgSource.Height - markImg.Height;
                    break;
                case WaterMarkLocationEnum.BottomRight:
                    x = imgSource.Width - markImg.Width;
                    y = imgSource.Height - markImg.Height;
                    break;
            }
        }
        #endregion

        #region 缩放水印
        /// <summary>
        /// 等比缩放水印图（缩小到原图的1/3）
        /// </summary>
        /// <param name="imgSource"></param>
        /// <param name="successImage"></param>
        /// <returns></returns>
        private static Image ImageShrink(Image imgSource, Image markImg)
        {
            int w = 0;
            int h = 0;

            Image.GetThumbnailImageAbort callb = null;

            //对水印图片生成缩略图,缩小到原图的1/3(以短的一边为准)                     
            if (imgSource.Width < imgSource.Height)
            {
                w = imgSource.Width / 3;
                h = markImg.Height * w / markImg.Width;
            }
            else
            {
                h = imgSource.Height / 3;
                w = markImg.Width * h / markImg.Height;
            }
            markImg = markImg.GetThumbnailImage(w, h, callb, new System.IntPtr());
            return markImg;
        }
        #endregion
    }
}
