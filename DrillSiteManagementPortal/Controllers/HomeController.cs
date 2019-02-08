using System.Web.Mvc;

namespace DrillSiteManagementPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // load data from DB
            // display data
            return View();
        }        
    }
}
