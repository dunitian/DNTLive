using System.Web.Mvc;
using System.Threading.Tasks;

namespace WMAPP.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var list = await FaceHelper.GetFaceModelList();
            return View();
        }

    }
}