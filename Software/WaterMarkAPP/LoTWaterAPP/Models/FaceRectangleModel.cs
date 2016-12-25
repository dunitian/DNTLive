namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：微软人脸识别API的脸部矩阵
    /// 描述：顶部左侧点坐标（left,top）,脸宽Width，脸高Height
    /// </summary>
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