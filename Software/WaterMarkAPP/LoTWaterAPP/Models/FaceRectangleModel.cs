namespace WMAPP.Models
{
    public partial class FaceRectangleModel
    {
        /// <summary>
        /// 脸的宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 脸的高度
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