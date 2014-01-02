using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quick5.Models;

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

            return View(table);
        }
    }
}
