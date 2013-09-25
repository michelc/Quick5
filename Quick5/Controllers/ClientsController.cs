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
    }
}
