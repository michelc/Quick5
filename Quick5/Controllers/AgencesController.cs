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

        public ViewResult Details(int id)
        {
            var agence = db.Agences.Get(id);

            return View(agence);
        }
    }
}
