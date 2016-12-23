using System.Text;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace WMAPP.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var msg = await MakeRequest();
            Response.Write(msg);
            return View();
        }

        public static async Task<HttpResponseMessage> MakeRequest()
        {
            var client = new HttpClient();
            //请求头
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "827ed8461d9b4b649fc4c111d7cd7f75");

            //请求参数
            var uri = "https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=true";

            //请求主体（可以是图片URL的json格式，也可以是图片类型）
            byte[] byteData = Encoding.UTF8.GetBytes("{url:'https://images2015.cnblogs.com/blog/658978/201609/658978-20160922111329527-2030285818.png'}");

            using (var content = new ByteArrayContent(byteData))
            {
                //内容类型
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                return await client.PostAsync(uri, content);
            }
        }
    }
}