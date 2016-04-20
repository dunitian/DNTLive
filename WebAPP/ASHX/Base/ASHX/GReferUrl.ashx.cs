using System.Web;

namespace CSharpStudy
{
    /// <summary>
    /// 07.防盗链或网络营销
    /// </summary>
    public class GReferUrl : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var uri = context.Request.UrlReferrer;//注意一下可能为空
            if (uri != null)
            {
                context.Response.Write(string.Format("来源URL：", uri));
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}