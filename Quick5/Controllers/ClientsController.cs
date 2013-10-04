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

        public ActionResult Index(string q)
        {
            var clients = new List<Client>();
            if (!string.IsNullOrEmpty(q))
            {
                if (q.Length > 3)
                {
                    var db = new ExtraBase();
                    clients = db.GetClients(q).ToList();
                }
            }

            return View(clients);
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            var db = new ExtraBase();
            var client = db.GetClient(id);

            client.Siren = db.Sirens.List(client.NSiren).FirstOrDefault();
            client.Garantie = db.GetGaranties(Client_ID: client.Client_ID).FirstOrDefault();

            return View(client);
        }
    }
}
