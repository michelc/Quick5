using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class ClientsController : Controller
    {
        //
        // GET: /Clients/

        public ActionResult Index(string qc)
        {
            var clients = new List<Client>();
            if (!string.IsNullOrEmpty(qc))
            {
                if (qc.Length >= 3)
                {
                    var db = new ExtraBase();
                    clients = db.Clients.List(qc);

                    if (clients.Count() == 1)
                    {
                        var id = clients.First().Client_ID;
                        return RedirectToAction("Details", new { id });
                    }
                }
            }

            return View(clients);
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var client = db.Clients.Get(id);

            client.Siren = db.Sirens.List(client.NSiren).FirstOrDefault();
            client.Garantie = db.Garanties.List(client.Client_ID).FirstOrDefault();
            if (client.Garantie == null) client.Garantie = new Garantie();

            return View(client);
        }
    }
}
