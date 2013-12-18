using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class DecisionsController : BaseController
    {
        //
        // GET: /Decisions/Details/5

        public ActionResult Details(int id)
        {
            var decision = db.Decisions.Get(id);

            decision.Siren = db.Sirens.List(decision.NSiren).FirstOrDefault();

            return View(decision);
        }
    }
}
