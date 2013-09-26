using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;
using StackExchange.Profiling;

namespace Quick5.Controllers
{
    public class SirensController : Controller
    {
        //
        // GET: /Sirens/

        public ActionResult Index(string q)
        {
            var sirens = new List<Siren>();
            if (!string.IsNullOrEmpty(q))
            {
                if (q.Length > 3)
                {
                    var db = new SqlExtra();
                    sirens = db.GetSirens(q).ToList();
                }
            }

            return View(sirens);
        }

        //
        // GET: /Sirens/Details/5

        public ViewResult Details(int id)
        {
            var db = new SqlExtra();

            var profiler = MiniProfiler.Current;

            Siren siren = null;
            using (profiler.Step("Siren"))
            {
                siren = db.GetSiren(id);
            }

            using (profiler.Step("Clients"))
            {
                siren.Clients = db.GetClients("", siren.NSiren);
            }

            using (profiler.Step("Garanties"))
            {
                siren.Garanties = db.GetGaranties(Siren_ID: siren.Siren_ID);
            }

            using (profiler.Step("Decisions"))
            {
                siren.Decisions = db.GetDecisions(Siren_ID: siren.Siren_ID);
            }

            return View(siren);
        }

        //
        // GET: /Sirens/Details/5

        public ViewResult Details_Backup(int id)
        {
            var db = new SqlExtra();
            var siren = db.GetSiren(id);

            siren.Clients = db.GetClients(siren.NSiren);
            siren.Garanties = db.GetGaranties(Siren_ID: siren.Siren_ID);
            siren.Decisions = db.GetDecisions(Siren_ID: siren.Siren_ID);

            return View(siren);
        }
    }
}
