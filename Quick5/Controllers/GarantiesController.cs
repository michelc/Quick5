using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class GarantiesController : BaseController
    {
        //
        // GET: /Garanties/Details/5

        public ViewResult Details(int id)
        {
            var garantie = db.Garanties.Get(id);

            garantie.Client = db.Clients.Get(garantie.Client_ID);
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            return View(garantie);
        }

        //
        // GET: /Garanties/Copy/5

        public ViewResult Copy(int id, int client_id = 0)
        {
            // Informations liées à la garantie à modifier
            var garantie = new Garantie();
            if (id != 0)
            {
                garantie = db.Garanties.Get(id);
                garantie.Client = db.Clients.Get(garantie.Client_ID);
            }
            else
            {
                garantie.Client = db.Clients.Get(client_id);
            }
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            // Information liées à une autre garantie du même Siren
            var a_recopier = db.Garanties.List(garantie.Siren.NSiren).Where(g => g.Garantie_ID != id).First();
            ViewBag.Recopier = a_recopier;

            return View(garantie);
        }

        //
        // GET: /Garanties/Copy/5

        [HttpPost]
        public ActionResult Copy(int id, int client_id = 0, string dum = "")
        {
            // Informations liées à la garantie à modifier
            var garantie = new Garantie();
            if (id != 0)
            {
                garantie = db.Garanties.Get(id);
                garantie.Client = db.Clients.Get(garantie.Client_ID);
            }
            else
            {
                garantie.Client = db.Clients.Get(client_id);
            }
            garantie.Siren = db.Sirens.List(garantie.Client.NSiren).FirstOrDefault();

            // Information liées à une autre garantie du même Siren
            var a_recopier = db.Garanties.List(garantie.Siren.NSiren).Where(g => g.Garantie_ID != id).First();

            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    a_recopier.Garantie_ID = id;
                    a_recopier.Client_ID = garantie.Client_ID;
                    db.Garanties.Update(a_recopier);

                    return RedirectToAction("Details", new { id });
                }
                else
                {
                    a_recopier.Client_ID = client_id;
                    db.Garanties.Insert(a_recopier);

                    return RedirectToAction("Details", "Clients", new { id = client_id });
                }
            }

            ViewBag.Recopier = a_recopier;
            return View(garantie);
        }
    }
}
