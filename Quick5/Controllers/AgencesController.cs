using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class AgencesController : BaseController
    {
        //
        // GET: /Agences/

        [OutputCache(Duration = 300)]
        public ActionResult Index()
        {
            var agences = db.Agences.List();

            return View(agences);
        }

        //
        // GET: /Agences/Details/5

        public ViewResult Details(int id, bool maj = false)
        {
            var agence = db.Agences.Get(id);

            ViewBag.Maj = maj;
            return View(agence);
        }

        //
        // POST: /Agences/UpdateUrssaf/5?Value=xxx

        [HttpPost]
        public RedirectToRouteResult UpdateUrssaf(int id, string value)
        {
            var code = id.ToString("000");
            if (string.IsNullOrEmpty(value)) value = "427 000000";
            db.ExecuteSql("UPDATE Agences SET Urssaf = :value WHERE Code_Agn = :code", new { code, value });

            return RedirectToAction("Details", new { id });
        }

        //
        // POST: /Agences/UpdateSiretUrssaf/5?Value=xxx

        [HttpPost]
        public RedirectToRouteResult UpdateSiretUrssaf(int id, string value)
        {
            var code = id.ToString("000");
            if (string.IsNullOrEmpty(value)) value = "33999316490000";
            db.ExecuteSql("UPDATE Agences SET Siret_Urssaf = :value WHERE Code_Agn = :code", new { code, value });

            return RedirectToAction("Details", new { id });
        }
    }
}
