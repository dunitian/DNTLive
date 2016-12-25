namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：脸部四个关键点
    /// 描述：X最小值即是脸部最左侧，X最大值即脸部最右侧，Y最大值即下巴位置，Y最小值-（YMax和YMin差值）即额头的值
    /// </summary>
    public partial class FaceFourKey
    {
        /// <summary>
        /// X坐标最小值
        /// </summary>
        public int XMin { get; set; }
        /// <summary>
        /// X坐标最大值
        /// </summary>
        public int XMax { get; set; }
        /// <summary>
        /// Y坐标最小值
        /// </summary>
        public int YMin { get; set; }
        /// <summary>
        /// Y坐标最大值
        /// </summary>
        public int YMax { get; set; }
    }
}