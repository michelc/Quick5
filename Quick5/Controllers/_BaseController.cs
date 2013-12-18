using System.Web.Mvc;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class BaseController : Controller
    {
        public ExtraBase db = new ExtraBase();
    }
}
