using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class EdiAccordsController : Controller
    {
        //
        // GET: /EdiAccords/

        public ActionResult Index()
        {
            var accords = new List<EdiAccord>();
            var db = new ExtraBase();
            accords = db.EdiAccords.List().ToList();

            return View(accords);
        }

        //
        // GET: /EdiAccords/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var accord = db.EdiAccords.Get(id);

            accord.Sites = db.EdiSites.List(accord.Accord_ID);

            return View(accord);
        }
    }
}
