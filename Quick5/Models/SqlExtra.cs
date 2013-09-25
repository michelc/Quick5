using System.Collections.Generic;
using Dapper;

namespace Quick5.Models
{
    public class SqlExtra : SqlBase
    {
        public SqlExtra() : base("Extra") { }

        public IEnumerable<Client> GetClients(string q)
        {
            if (string.IsNullOrEmpty(q)) return new List<Client>();
            if (q.Length <= 3) return new List<Client>();

            IEnumerable<Client> data = null;

            var sql = @"SELECT IdCompany AS Client_ID
                             , Name AS Nom
                             , Siren
                             , Fld109 AS Siret
                             , PostCode AS CodePostal
                             , City AS Ville
                             , Fld138 AS Type
                             , DECODE(Fld129, NULL, 0, -1) AS EstBloque
                        FROM   Cy
                        WHERE  (UPPER(Name) LIKE '%{q}%')
                        OR     (Fld109 LIKE '{q}%')
                        ORDER BY UPPER(Name)
                               , UPPER(City)";
            sql = sql.Replace("{q}", q.ToUpperInvariant());

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
    }
}