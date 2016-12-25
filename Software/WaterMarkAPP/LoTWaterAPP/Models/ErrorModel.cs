namespace WMAPP.Models
{
    /// <summary>
    /// 作者：dunitian
    /// 时间：2016年12月23日 10:40
    /// 标题：错误信息
    /// 描述：返回的错误信息
    /// </summary>
    public partial class ErrorModel
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
    }
}