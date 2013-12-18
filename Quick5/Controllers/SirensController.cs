using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class SirensController : Controller
    {
        //
        // GET: /Sirens/

        public ActionResult Index(string qs)
        {
            var sirens = new List<Siren>();
            if (!string.IsNullOrEmpty(qs))
            {
                if (qs.Length >= 3)
                {
                    var db = new ExtraBase();
                    sirens = db.Sirens.List(qs);

                    if (sirens.Count() == 1)
                    {
                        var id = sirens.First().Siren_ID;
                        return RedirectToAction("Details", new { id });
                    }
                }
            }

            return View(sirens);
        }

        //
        // GET: /Sirens/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var siren = db.Sirens.Get(id);

            siren.Clients = db.Clients.List(siren.NSiren);
            siren.Garanties = db.Garanties.List(siren.NSiren);
            siren.Decisions = db.Decisions.List(siren.NSiren);

            return View(siren);
        }
    }
}
