using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class SirensController : BaseController
    {
        //
        // GET: /Sirens/

        public ActionResult Index(string qs = "")
        {
            var sirens = db.Sirens.List(qs);

            if (sirens.Count() == 1)
            {
                var id = sirens.First().Siren_ID;
                return RedirectToAction("Details", new { id });
            }

            return View(sirens);
        }

        //
        // GET: /Sirens/Details/5

        public ViewResult Details(int id)
        {
            var siren = db.Sirens.Get(id);

            siren.Clients = db.Clients.List(siren.NSiren);
            siren.Garanties = db.Garanties.List(siren.NSiren);
            siren.Decisions = db.Decisions.List(siren.NSiren);

            return View(siren);
        }

        //
        // GET: /Sirens/Api/5
        // http://localhost:62636/Sirens/Api/44120

        public ViewResult Api(int id)
        {
            var siren = db.Sirens.Get(id);

            var prm = string.Format(ConfigurationManager.AppSettings.Get("api_prm"),
                                    DateTime.Now.ToString("yyyMMdd"), 
                                    DateTime.Now.ToString("HHmm"));
            var md5 = ConvertMD5(prm + ConfigurationManager.AppSettings.Get("api_mdp"));
            var url = string.Format(ConfigurationManager.AppSettings.Get("api_url"),
                                    prm,
                                    md5,
                                    siren.NSiren);

            string text = "";
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                text = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                text = ex.Message;
            }

            ViewBag.Url = url;
            ViewBag.Text = text;
            return View(siren);
        }

        public string ConvertMD5(string value)
        {
            var md = MD5.Create();
            var data = md.ComputeHash(Encoding.Default.GetBytes(value));
            var hash = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                hash.Append(data[i].ToString("x2"));
            }
            return hash.ToString().ToLower();
        }
    }
}
