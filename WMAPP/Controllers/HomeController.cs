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

            }
            catch (FaceException ex)
            {
                ViewBag.ErrorInfo = ex.Message;
            }


            return View();
        }

    }
}