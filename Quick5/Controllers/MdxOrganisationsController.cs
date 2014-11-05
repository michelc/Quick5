using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class MdxOrganisationsController : BaseController
    {
        //
        // GET: /MdxOrganisations/

        [OutputCache(Duration = 300)]
        public ActionResult Index()
        {
            var organisations = db.MdxOrganisations.List();

            return View(organisations);
        }

        //
        // GET: /MdxOrganisations/Details/5

        public ViewResult Details(string id)
        {
            var organisation = db.MdxOrganisations.Get(id);

            return View(organisation);
        }
    }
}
