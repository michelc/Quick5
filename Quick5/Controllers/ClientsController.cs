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
            var db = new SqlExtra();
            var clients = db.GetClients(q);

            return View(clients);
        }

        //
        // GET: /Clients/Details/5

        public ViewResult Details(int id)
        {
            var db = new SqlExtra();
            var client = db.GetClient(id);

            return View(client);
        }
    }
}
