using System.Linq;
using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class CommunesController : BaseController
    {
        //
        // GET: /Communes/

        public ActionResult Index(string qc = "")
        {
            var communes = db.Communes.List(qc);

            if (communes.Count() == 1)
            {
                var id = communes.First().Commune_ID;
                return RedirectToAction("Details", new { id });
            }

            return View(communes);
        }

        //
        // GET: /Communes/Details/5

        public ViewResult Details(int id)
        {
            var commune = db.Communes.Get(id);

            return View(commune);
        }
    }
}
