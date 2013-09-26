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

        public IEnumerable<Garantie> GetGaranties(int Siren_ID = 0, int Client_ID = 0, int Garantie_ID = 0)
        {
            IEnumerable<Garantie> data = null;

            var sql = @"SELECT R.Risque_ID AS Garantie_ID
                             , R.Client_ID
                             , R.Montant_Risque AS GarMontant
                             , R.Option_Risque AS GarOption
                             , R.Date_Risque AS GarDate
                             , R.Periode_Debut AS GarDebut
                             , R.Periode_Fin AS GarFin
                             , R.Montant_Risque_Compl AS CplMontant
                             , R.Date_Debut_Risque AS CplDebut
                             , R.Date_Fin_Risque AS CplFin
                             , R.Garantie_Interne AS IntMontant
                             , R.Garantie_Periode_Debut AS IntDebut
                             , R.Garantie_Periode_Fin AS IntFin
                             , R.Mnt_Oal AS OalMontant
                             , R.Dte_Debut_Oal AS OalDebut
                             , R.Dte_Fin_Oal AS OalFin
                             , R.Mnt_Cap AS CapMontant
                             , R.Option_Cap AS CapOption
                             , R.Dte_Debut_Cap AS CapDebut
                             , R.Dte_Fin_Cap AS CapFin
                             , R.Montant_Deblocage AS Deblocage
                             , R.Montant_Risque
                             + R.Montant_Risque_Compl
                             + R.Garantie_Interne
                             + R.Mnt_Oal
                             + R.Mnt_Cap
                             + R.Montant_Deblocage AS Totale
                        FROM   Ct_Risques_Clients R ";

            if (Garantie_ID != 0)
            {
                sql += "WHERE R.Risque_ID = " + Garantie_ID.ToString();
            }
            else if (Client_ID != 0)
            {
                sql += "WHERE R.Client_ID = " + Client_ID.ToString();
            }
            else
            {
                sql += @"    , Cy C
                             , Ct_Fiche_Siren S
                         WHERE (S.ID = {siren_id})
                         AND   (C.Siren = S.Siren)
                         AND   (R.Client_ID = C.IdCompany)
                         ORDER BY UPPER(C.Name)
                                , UPPER(C.City)";
                sql = sql.Replace("{siren_id}", Siren_ID.ToString());
            }

            try
            {
                connexion.Open();
                data = connexion.Query<Garantie>(sql);
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