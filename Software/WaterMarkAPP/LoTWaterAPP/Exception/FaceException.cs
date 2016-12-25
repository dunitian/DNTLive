namespace WaterWaterWaterMark
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：微软人脸识别API的专用异常
    /// </summary>
    public class FaceException : System.Exception
    {
        public FaceException(string message) : base(message)
        {
        }
    }
}