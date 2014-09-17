using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class TablesController : BaseController
    {
        //
        // GET: /Tables/

        public ActionResult Index()
        {
            var tables = db.Tables.List();

            return View(tables);
        }

        //
        // GET: /Tables/Details/Xxx

        public ViewResult Details(string id)
        {
            var table = db.Tables.Get(id);

            return View(table);
        }

        //
        // GET: /Tables/Content/Xxx

        public ViewResult Content(string id)
        {
            var table = db.Tables.Execute("SELECT * FROM " + id);

            ViewBag.TableName = id;
            return View(table);
        }
    }
}
