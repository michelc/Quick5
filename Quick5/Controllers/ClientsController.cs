using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class ClientsController : Controller
    {
        //
        // GET: /Clients/

        public ActionResult Index()
        {
            var db = new SqlExtra();
            var clients = db.GetClients("343691374");

            return View(clients);
        }
    }
}
