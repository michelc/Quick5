using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class DecisionsController : Controller
    {
        //
        // GET: /Decisions/Details/5

        public ActionResult Details(int id)
        {
            var db = new ExtraBase();
            var decision = db.Decisions.Get(id);

            decision.Siren = db.Sirens.List(decision.NSiren).FirstOrDefault();

            return View(decision);
        }
    }
}
