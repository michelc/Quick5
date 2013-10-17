using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class AgencesController : Controller
    {
        //
        // GET: /Agences/

        [OutputCache(Duration = 300)]
        public ActionResult Index()
        {
            var agences = new List<Agence>();
            var db = new ExtraBase();
            agences = db.Agences.List().ToList();

            return View(agences);
        }

        //
        // GET: /Agences/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var agence = db.Agences.Get(id);

            return View(agence);
        }
    }
}
