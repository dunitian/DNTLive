using System.Web.Mvc;
using System.Threading.Tasks;

namespace WMAPP.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            try
            {
                var list = await FaceHelper.GetFaceModelList("https://images2015.cnblogs.com/blog/658978/201609/658978-20160922111329527-2030285818.png");
                //todo:web端的处理（我这边就不弄了，需求是搞PC版的处理）
                ViewBag.InfoList = await list.ObjectToJsonAsync();
            }
            catch (FaceException ex)
            {
                ViewBag.ErrorInfo = ex.Message;
            }


            return View();
        }

    }
}