using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Quick5.Controllers
{
    public class InseesController : BaseController
    {
        //
        // GET: /Insees/

        public ActionResult Index(string qc = "")
        {
            var insees = db.Insees.List(qc);

            if (insees.Count() == 1)
            {
                var id = insees.First().ID;
                return RedirectToAction("Details", new { id });
            }

            return View(insees);
        }

        //
        // GET: /Insees/Details/5

        public ViewResult Details(string id)
        {
            var insee = db.Insees.Get(id);

            return View(insee);
        }

        //
        // GET: /Insees/Import

        public ViewResult Import()
        {
            return View();
        }

        //
        // POT: /Insees/Import

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var name = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data"), name);
                file.SaveAs(path);

                // Vide la table des communes Insee
                db.ExecuteSql("TRUNCATE TABLE Mcr_Insees");

                // Insère le contenu du fichier Insee par paquet de 1000
                var count = 0;
                var batch = new StringBuilder();
                batch.Append("BEGIN ");
                var sql = "INSERT INTO Mcr_Insees (ID, Nom) VALUES ('{0}', '{1}'); ";
                foreach (var line in System.IO.File.ReadLines(path))
                {
                    if (count > 0)
                    {
                        var cols = line.Split('\t');
                        var id = cols[3] + cols[4];
                        id = id.Trim();
                        var nom = "-" + cols[8] + "-" + cols[9];
                        nom = nom.Replace("(", "");
                        nom = nom.Replace(")", "");
                        nom = nom.Replace("-SAINT-", "-ST-");
                        nom = nom.Replace("-SAINTE-", "-STE-");
                        nom = nom.Replace("-", " ");
                        nom = nom.Replace("'", " ");
                        nom = nom.Replace("  ", " ").Replace("  ", " ").Trim();
                        batch.AppendFormat(sql, id, nom);
                    }
                    count++;
                    if ((count % 1000) == 0)
                    {
                        batch.Append("END;");
                        db.ExecuteSql(batch.ToString());
                        batch = new StringBuilder();
                        batch.Append("BEGIN ");
                    }
                }
                // Corrige doublons Bors et Castillon
                batch.Append("UPDATE Mcr_Insees SET Nom = 'BORS DE MONTMOREAU' WHERE ID = '16052' AND Nom = 'BORS CANTON DE MONTMOREAU ST CYBARD';");
                batch.Append("UPDATE Mcr_Insees SET Nom = 'BORS DE BAIGNES' WHERE ID = '16053' AND Nom = 'BORS CANTON DE BAIGNES STE RADEGONDE';");
                batch.Append("UPDATE Mcr_Insees SET Nom = 'CASTILLON D ARTHEZ' WHERE ID = '64181' AND Nom = 'CASTILLON CANTON D ARTHEZ DE BEARN';");
                batch.Append("UPDATE Mcr_Insees SET Nom = 'CASTILLON DE LEMBEYE' WHERE ID = '64182' AND Nom = 'CASTILLON CANTON DE LEMBEYE';"); 
                batch.Append("END;");
                db.ExecuteSql(batch.ToString());
                count += 0;
            }

            return RedirectToAction("Index");
        }
    }
}
