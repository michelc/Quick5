using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace Quick5.Models
{
    public class SqlExtra : SqlBase
    {
        public SqlExtra() : base("Extra") { }

        public IEnumerable<Client> GetClients(string q)
        {
            IEnumerable<Client> data = null;

            var sql = @"SELECT IdCompany AS Client_ID
                             , Name AS Nom
                             , Siren AS NSiren
                             , Fld109 AS NSiret
                             , PostCode AS CodePostal
                             , City AS Ville
                             , Fld138 AS Type
                             , DECODE(Fld129, NULL, 0, -1) AS EstBloque
                        FROM   Cy
                        WHERE  (UPPER(Name) LIKE '%{nom}%')
                        OR     (Siren LIKE '{siren}%')
                        OR     (Fld109 LIKE '{siren}%')
                        ORDER BY UPPER(Name)
                               , UPPER(City)";
            sql = sql.Replace("{nom}", q.ToUpperInvariant());
            sql = sql.Replace("{siren}", q.ToUpperInvariant().Replace(" ", ""));

            try
            {
                connexion.Open();
                data = connexion.Query<Client>(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            return data;
        }

        public Client GetClient(int id)
        {
            var data = new Client();

            var sql = @"SELECT IdCompany AS Client_ID
                             , Name AS Nom
                             , Siren AS NSiren
                             , Fld109 AS NSiret
                             , PostCode AS CodePostal
                             , City AS Ville
                             , Fld138 AS Type
                             , DECODE(Fld129, NULL, 0, -1) AS EstBloque
                        FROM   Cy
                        WHERE  IdCompany = " + id.ToString();

            try
            {
                connexion.Open();
                data = connexion.Query<Client>(sql).FirstOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            return data;
        }

        public IEnumerable<Siren> GetSirens(string q)
        {
            IEnumerable<Siren> data = null;

            var sql = @"SELECT ID AS Siren_ID
                             , Raison_Social AS Nom
                             , Siren AS NSiren
                             , DECODE(Blocage, NULL, 0, -1) AS EstBloque
                        FROM   Ct_Fiche_Siren
                        WHERE  (Societe_ID = '001')
                        AND    ((UPPER(Raison_Social) LIKE '%{nom}%') OR (Siren LIKE '{siren}%'))
                        ORDER BY UPPER(Raison_Social)
                               , Siren";
            sql = sql.Replace("{nom}", q.ToUpperInvariant());
            sql = sql.Replace("{siren}", q.ToUpperInvariant().Replace(" ", ""));

            try
            {
                connexion.Open();
                data = connexion.Query<Siren>(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            return data;
        }

        public Siren GetSiren(int id)
        {
            var data = new Siren();

            var sql = @"SELECT ID AS Siren_ID
                             , Raison_Social AS Nom
                             , Siren AS NSiren
                             , DECODE(Blocage, NULL, 0, -1) AS EstBloque
                        FROM   Ct_Fiche_Siren
                        WHERE  ID = " + id.ToString();

            try
            {
                connexion.Open();
                data = connexion.Query<Siren>(sql).FirstOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            return data;
        }
    }
}