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
            var siren = db.GetSiren(id);

            return View(siren);
        }
    }
}
