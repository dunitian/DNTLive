namespace WMAPP.Models
{
    public partial class FaceRectangleModel
    {
        /// <summary>
        /// 宽
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 顶部左侧点-x坐标
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// 顶部左侧点-Y坐标
        /// </summary>
        public int Top { get; set; }
    }
}