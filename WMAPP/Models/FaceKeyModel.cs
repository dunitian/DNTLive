namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：人脸关键点（轮廓点）逆天小算法：（考虑不能全识别出来的情况）
    /// 描述：[四个关键点：X最小值即是脸部最左侧，X最大值即脸部最右侧，Y最大值即下巴位置，Y最小值-（YMax和YMin差值）即额头的值]
    /// 描述：[以脸部中心为轴心，取XMax和YMax的最大值*1.5 为正方形边（要考虑侧脸），来画一方免水印领域]
    /// </summary>
    public partial class FaceKeyModel
    {
        #region 常规点

        #region 下巴关键点
        public FaceXY Contour_Chin { get; set; }
        #endregion

        #region 左侧轮廓点
        public FaceXY Contour_Left1 { get; set; }
        public FaceXY Contour_Left2 { get; set; }
        public FaceXY Contour_Left3 { get; set; }
        public FaceXY Contour_Left4 { get; set; }
        public FaceXY Contour_Left5 { get; set; }
        public FaceXY Contour_Left6 { get; set; }
        public FaceXY Contour_Left7 { get; set; }
        public FaceXY Contour_Left8 { get; set; }
        public FaceXY Contour_Left9 { get; set; }
        #endregion

        #region 右侧侧轮廓点
        public FaceXY Contour_Right1 { get; set; }
        public FaceXY Contour_Right2 { get; set; }
        public FaceXY Contour_Right3 { get; set; }
        public FaceXY Contour_Right4 { get; set; }
        public FaceXY Contour_Right5 { get; set; }
        public FaceXY Contour_Right6 { get; set; }
        public FaceXY Contour_Right7 { get; set; }
        public FaceXY Contour_Right8 { get; set; }
        public FaceXY Contour_Right9 { get; set; }
        #endregion

        #endregion

        #region 核心点
        /// <summary>
        /// 脸部四个关键点（自己算）
        /// </summary>
        public FaceFourKey FourKey { get; set; }

        /// <summary>
        /// 脸部中心点
        /// </summary>
        public FaceXY FaceCentral { get; set; }

        /// <summary>
        /// 脸部新四个关键点（备用属性）
        /// </summary>
        public FaceFourKey NewFourKey { get; set; }
        #endregion

        #region 小备用
        /// <summary>
        /// 备用属性
        /// </summary>
        public object Other { get; set; }
        #endregion
    }
}