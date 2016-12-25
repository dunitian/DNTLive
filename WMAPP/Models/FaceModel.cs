namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：微软人脸识别API返回值类型
    /// 描述：其他的信息暂时不需要
    /// </summary>
    public partial class FaceModel
    {
        /// <summary>
        /// Face对应ID
        /// </summary>
        public string FaceId { get; set; }
        /// <summary>
        /// 脸部矩阵
        /// </summary>
        public FaceRectangleModel FaceRectangLe { get; set; }

        //FaceLandmarks 这边暂时不需要
    }
}