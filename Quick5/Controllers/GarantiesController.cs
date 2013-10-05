using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class GarantiesController : Controller
    {
        //
        // GET: /Garanties/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var garantie = db.Garanties.Get(id);

            garantie.Client = db.Clients.Get(garantie.Client_ID);
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            return View(garantie);
        }
    }
}
