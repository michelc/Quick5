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
            var db = new SqlExtra();
            var garantie = db.GetGaranties(Garantie_ID: id).FirstOrDefault();

            garantie.Client = db.GetClient(garantie.Client_ID);
            garantie.Siren = db.GetSirens(garantie.Client.NSiren).FirstOrDefault();

            return View(garantie);
        }
    }
}
