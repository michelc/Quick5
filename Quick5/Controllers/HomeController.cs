using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Quick5.Models;

namespace Quick5.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) return View();

            return Content("Quick5...");
        }

        //
        // GET: /Home/Logon/xxx

        [AllowAnonymous]
        public ActionResult Logon(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            return View();
        }

        //
        // POST: /Home/Logon/xxx

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Logon(string id, string pw)
        {
            if (!FormsAuthentication.Authenticate(id, pw)) return RedirectToAction("Index");

            FormsAuthentication.SetAuthCookie(id, true);

            return RedirectToAction("Index");
        }

        //
        // GET: /Home/Logout

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index");
        }

        //
        // GET: /Home/Init

        public ActionResult Init()
        {
            RecreateDbTests(db);

            return RedirectToAction("Index");
        }

        private void RecreateDbTests(ExtraBase db)
        {
            // Rien à faire s'il ne s'agit pas de la base de données de tests
            if (!MvcApplication.IsDbTests) return;

            // Ouverture de la base de données
            db.Open();

            // Suppression des tables existantes
            db.ExecuteSql("DROP TABLE Ct_Fiche_Siren");
            db.ExecuteSql("DROP TABLE Cy");
            db.ExecuteSql("DROP TABLE Agences");
            db.ExecuteSql("DROP TABLE Ct_Risques_Clients");
            db.ExecuteSql("DROP TABLE Ct_Historique_Atradius");
            db.ExecuteSql("DROP TABLE Accord_Edi_National");
            db.ExecuteSql("DROP TABLE Etablissement_Edi");

            // Création des tables
            db.ExecuteSql(SqlCreate("Ct_Fiche_Siren", typeof(DbSiren)));
            db.ExecuteSql(SqlCreate("Cy", typeof(DbClient)));
            db.ExecuteSql(SqlCreate("Agences", typeof(DbAgence)));
            db.ExecuteSql(SqlCreate("Ct_Risques_Clients", typeof(DbGarantie)));
            db.ExecuteSql(SqlCreate("Ct_Historique_Atradius", typeof(DbDecision)));
            db.ExecuteSql(SqlCreate("Accord_Edi_National", typeof(DbEdiAccord)));
            db.ExecuteSql(SqlCreate("Etablissement_Edi", typeof(DbEdiSite)));

            // Alimentation table des sirens
            new List<DbSiren>
            {
                new DbSiren { Societe_ID = "001", Raison_Social = "ETS AZERTY", Siren = "111111111", Blocage = null },
                new DbSiren { Societe_ID = "001", Raison_Social = "BIM BAM BOUM", Siren = "222222222", Blocage = null },
                new DbSiren { Societe_ID = "001", Raison_Social = "AM STRAM GRAM", Siren = "333333333", Blocage = "03" }
            }.ForEach(s => db.connexion.Insert<DbSiren>(s));

            // Alimentation table des clients
            new List<DbClient>
            {
                new DbClient { Name = "ETS AZERTY", Siren = "111111111", Fld109 = "11111111100001", PostCode = "69001", City = "LYON", Fld138 = "Client", Fld129 = null },
                new DbClient { Name = "SARL AZERTY", Siren = "111111111", Fld109 = "11111111100002", PostCode = "75001", City = "PARIS", Fld138 = "Client", Fld129 = "01" },
                new DbClient { Name = "BIM BAM BOUM", Siren = "222222222", Fld109 = "22222222200001", PostCode = "69002", City = "LYON", Fld138 = "Client", Fld129 = null },
                new DbClient { Name = "BBB CAPITALE", Siren = "222222222", Fld109 = "22222222200002", PostCode = "75002", City = "PARIS", Fld138 = "Prospect", Fld129 = null },
                new DbClient { Name = "BIM BAM BOUM", Siren = "222222222", Fld109 = "22222222200003", PostCode = "13002", City = "MARSEILLE", Fld138 = "Prospect", Fld129 = "02" }
            }.ForEach(c => db.connexion.Insert<DbClient>(c));

            // Alimentation table des agences
            new List<DbAgence>
            {
                new DbAgence { Societe_ID = "001", Code_Agn = "001", Libelle = "LYON", Departement = "69001" },
                new DbAgence { Societe_ID = "001", Code_Agn = "002", Libelle = "PARIS", Departement = "75002" }
            }.ForEach(a =>
            {
                db.connexion.Insert<DbAgence>(a);
                db.ExecuteSql("UPDATE Agences SET Code_Agn = @Code_Agn WHERE Libelle = @Libelle", a);
            });

            // Alimentation table des garanties
            new List<DbGarantie>
            {
                new DbGarantie { Client_ID = 1, Montant_Risque = 10000, Option_Risque = "OK", Date_Risque = DateTime.Now.AddDays(-150).Date },
                new DbGarantie { Client_ID = 2, Montant_Risque = 10000, Option_Risque = "OK", Date_Risque = DateTime.Now.AddDays(-150).Date },
                new DbGarantie { Client_ID = 3, Montant_Risque = 20000, Option_Risque = "OK2", Date_Risque = DateTime.Now.AddDays(-100).Date, Montant_Risque_Compl = 2000 },
                new DbGarantie { Client_ID = 4, Montant_Risque = 20000, Option_Risque = "OK2", Date_Risque = DateTime.Now.AddDays(-100).Date, Montant_Risque_Compl = 2000 },
                new DbGarantie { Client_ID = 5, Montant_Risque = 20000, Option_Risque = "OK2", Date_Risque = DateTime.Now.AddDays(-100).Date, Montant_Risque_Compl = 2222 }
            }.ForEach(g => db.connexion.Insert<DbGarantie>(g));

            // Alimentation table des accords edi
            new List<DbEdiAccord>
            {
                new DbEdiAccord { Libelle = "BIM BAM BOUM", Siren = "222222222", Code_Externe_Eu = "222222222" },
                new DbEdiAccord { Libelle = "AZERTY", Siren = "111111111", Code_Externe_Eu = "AZERTY" }
            }.ForEach(a => db.connexion.Insert<DbEdiAccord>(a));

            // Alimentation des sites edi
            new List<DbEdiSite>
            {
                new DbEdiSite { Accord_National_Id = 1, Libelle = "PARIS", Siret = "22222222200002", Code_Externe_Eu = "00002" },
                new DbEdiSite { Accord_National_Id = 1, Libelle = "LYON", Siret = "22222222200001", Code_Externe_Eu = "00001" },
                new DbEdiSite { Accord_National_Id = 2, Libelle = "ETS AZERTY LYON", Siret = "11111111100001", Code_Externe_Eu = "LYON" },
                new DbEdiSite { Accord_National_Id = 2, Libelle = "AZERTY MARSEILLE", Siret = "11111111100003", Code_Externe_Eu = "MARSEILLE" }
            }.ForEach(s => db.connexion.Insert<DbEdiSite>(s));
 
            // Fermeture de la base de données
            db.Close();
        }

        /// <summary>
        /// Script SQL minimaliste pour créer une table
        /// </summary>
        private string SqlCreate(string name, Type type)
        {
            var sql = new StringBuilder();
            sql.Append("CREATE TABLE ");
            sql.Append(name);
            sql.Append(" (");

            var properties = type.GetProperties().ToArray();
            var first = true;
            foreach (var p in properties)
            {
                if (!first) sql.Append(",");
                sql.Append(p.Name);
                var type_name = p.PropertyType.Name;
                if (type_name.StartsWith("Null")) type_name = "DateTime?";
                switch (type_name)
                {
                    case "String":
                        sql.Append(" NVARCHAR(255)");
                        break;
                    case "DateTime":
                    case "DateTime?":
                        sql.Append(" DATETIME");
                        break;
                    case "Decimal":
                        sql.Append(" MONEY");
                        break;
                    default:
                        sql.Append(" INT NOT NULL");
                        if (first) sql.Append(" IDENTITY (1,1)");
                        break;
                }
                first = false;
            }

            sql.Append(" )");
            return sql.ToString();
        }
    }
}
