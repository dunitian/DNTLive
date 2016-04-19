using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WaterMarkAPP.Enums;

namespace WaterMarkAPP.Model
{
    /// <summary>
    /// 水印类（重要参数：原图类型，水印类型，水印位置，图片水印路径，文字水印内容）
    /// </summary>
    public class WaterMark
    {
        #region 基础属性设置
        private string _photoType;
        /// <summary>
        /// 原图类型
        /// </summary>
        public string PhotoType
        {
            get { return _photoType; }
            set { _photoType = value; }
        }

        private WaterMarkTypeEnum _waterMarkType = WaterMarkTypeEnum.NoneMark;
        /// <summary>
        /// 水印类型
        /// </summary>
        public WaterMarkTypeEnum WaterMarkType
        {
            get { return _waterMarkType; }
            set { _waterMarkType = value; }
        } 
        #endregion

        #region 水印效果设置
        private WaterMarkLocationEnum _waterMarkLocation = WaterMarkLocationEnum.BottomRight;
        /// <summary>
        /// 水印位置
        /// </summary>
        public WaterMarkLocationEnum WaterMarkLocation
        {
            get { return _waterMarkLocation; }
            set { _waterMarkLocation = value; }
        }

        private float _transparency = 0.7f;
        /// <summary>
        /// 水印透明度
        /// </summary>
        public float Transparency
        {
            get { return _transparency; }
            set { _transparency = value; }
        } 
        #endregion

        #region 图片水印设置
        private string _imgPath;
        /// <summary>
        /// 图片水印路径
        /// </summary>
        public string ImgPath
        {
            get { return _imgPath; }
            set { _imgPath = value; }
        } 
        #endregion

        #region 文字水印设置
        private string _text = "dunitian";
        /// <summary>
        /// 文字水印内容
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string _fontFamily = "微软雅黑";
        /// <summary>
        /// 文字字体
        /// </summary>
        public string FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }

        private Brush _brushesColor = Brushes.Black;
        /// <summary>
        /// 文字颜色
        /// </summary>
        public Brush BrushesColor
        {
            get { return _brushesColor; }
            set { _brushesColor = value; }
        }

        private  FontStyle _fontStyle = FontStyle.Regular;
        /// <summary>
        /// 字体样式
        /// </summary>
        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }

        private float _fontSize = 14f;
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        } 
        #endregion
    }
}
