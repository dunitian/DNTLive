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
                var list = await FaceHelper.GetFaceModelList();
                //todo:web端的处理（我这边就不弄了，需求是搞PC版的处理）
            }
            catch (FaceException ex)
            {
                ViewBag.ErrorInfo = ex.Message;
            }


            return View();
        }

    }
}