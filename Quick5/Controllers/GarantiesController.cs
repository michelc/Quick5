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

        //
        // GET: /Garanties/Edit/5

        public ViewResult Edit(int id)
        {
            var db = new ExtraBase();

            // Informations liées à la garantie à modifier
            var garantie = db.Garanties.Get(id);
            garantie.Client = db.Clients.Get(garantie.Client_ID);
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            // Information liées à une autre garantie du même Siren
            var a_recopier = db.Garanties.List(garantie.Siren.NSiren).Where(g => g.Garantie_ID != id).First();
            ViewBag.Recopier = a_recopier;

            return View(garantie);
        }

        //
        // GET: /Garanties/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, string dum)
        {
            var db = new ExtraBase();

            // Informations liées à la garantie à modifier
            var garantie = db.Garanties.Get(id);
            garantie.Client = db.Clients.Get(garantie.Client_ID);
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            // Information liées à une autre garantie du même Siren
            var a_recopier = db.Garanties.List(garantie.Siren.NSiren).Where(g => g.Garantie_ID != id).First();

            if (ModelState.IsValid)
            {
                a_recopier.Garantie_ID = id;
                a_recopier.Client_ID = garantie.Client_ID;
                db.Garanties.Save(a_recopier);

                return RedirectToAction("Details", new { id });
            }

            ViewBag.Recopier = a_recopier;
            return View(garantie);
        }
    }
}
