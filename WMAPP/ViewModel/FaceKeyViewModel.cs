namespace WMAPP.ViewModel
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：API脸部关键点
    /// 描述：其他的信息暂时不需要
    /// </summary>
    public class FaceKeyViewModel
    {
        #region 下巴关键点
        public Models.FaceXY Contour_Chin { get; set; }
        #endregion

        #region 左侧轮廓点
        public Models.FaceXY Contour_Left1 { get; set; }
        public Models.FaceXY Contour_Left2 { get; set; }
        public Models.FaceXY Contour_Left3 { get; set; }
        public Models.FaceXY Contour_Left4 { get; set; }
        public Models.FaceXY Contour_Left5 { get; set; }
        public Models.FaceXY Contour_Left6 { get; set; }
        public Models.FaceXY Contour_Left7 { get; set; }
        public Models.FaceXY Contour_Left8 { get; set; }
        public Models.FaceXY Contour_Left9 { get; set; }
        #endregion

        #region 右侧侧轮廓点
        public Models.FaceXY Contour_Right1 { get; set; }
        public Models.FaceXY Contour_Right2 { get; set; }
        public Models.FaceXY Contour_Right3 { get; set; }
        public Models.FaceXY Contour_Right4 { get; set; }
        public Models.FaceXY Contour_Right5 { get; set; }
        public Models.FaceXY Contour_Right6 { get; set; }
        public Models.FaceXY Contour_Right7 { get; set; }
        public Models.FaceXY Contour_Right8 { get; set; }
        public Models.FaceXY Contour_Right9 { get; set; }
        #endregion
    }
}