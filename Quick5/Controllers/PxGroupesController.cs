using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class PxGroupesController : BaseController
    {
        //
        // GET: /PxGroupes/

        public ActionResult Index()
        {
            var groupes = db.PxGroupes.List();

            return View(groupes);
        }

        //
        // GET: /PxGroupes/Details/5

        public ViewResult Details(int id)
        {
            var groupe = db.PxGroupes.Get(id);

            groupe.Clients = db.Clients.List("px=" + groupe.Code);
            groupe.Sites = db.PxSites.List(groupe.Groupe_ID);

            return View(groupe);
        }
    }
}
