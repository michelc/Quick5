using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var db = new ExtraBase();
            ViewBag.Table = db.GetTableName(typeof(DbSiren));

            return View();
        }

    }
}
