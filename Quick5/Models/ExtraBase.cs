using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dapper;
using Quick5.Helpers;

namespace Quick5.Models
{
    public class ExtraBase : SqlBase
    {
        public ExtraBase() : base("Extra") { }

        public IEnumerable<Client> GetClients(string q)
        {
            IEnumerable<DbClient> data = null;

            var sql = @"SELECT IdCompany
                             , Name
                             , Siren
                             , Fld109
                             , PostCode
                             , City
                             , Fld138
                             , Fld129
                        FROM   Cy ";

            try
            {
                connexion.Open();

                var siren = Tools.DigitOnly(q);
                object param = null;
                if (siren.Length >= 14)
                {
                    // Recherche par n° siret
                    sql += "WHERE  (Fld109 = :Siret)";
                    param = new { Siret = siren.Substring(0, 14) };
                }
                else if (siren.Length >= 9)
                {
                    // Recherche par n° siren
                    sql += "WHERE  (Siren = :Siren)";
                    param = new { Siren = siren.Substring(0, 9) };
                }
                else if (siren.Length < 3)
                {
                    // Recherche par nom client seul
                    sql += "WHERE  (UPPER(Name) LIKE :Nom)";
                    param = new { Nom = "%" + q.ToUpperInvariant() + "%" };
                }
                else
                {
                    // Recherche par nom client ou n° siren
                    sql += "WHERE  ((UPPER(Name) LIKE :Nom) OR (Siren LIKE :Siren))";
                    param = new { Nom = "%" + q.ToUpperInvariant() + "%", Siren = siren + "%" };
                }
                sql += " ORDER BY UPPER(Name), UPPER(City)";
                data = connexion.Query<DbClient>(sql, param);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbClient>, IEnumerable<Client>>(data);
            return view_model;
        }

        public Client GetClient(int id)
        {
            var data = new DbClient();

            var sql = @"SELECT IdCompany
                             , Name
                             , Siren
                             , Fld109
                             , PostCode
                             , City
                             , Fld138
                             , Fld129
                        FROM   Cy
                        WHERE  IdCompany = :Id";

            try
            {
                connexion.Open();
                data = connexion.Query<DbClient>(sql, new { id }).FirstOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<Client>(data);
            return view_model;
        }

        public IEnumerable<Siren> GetSirens(string q)
        {
            IEnumerable<DbSiren> data = null;

            try
            {
                connexion.Open();

                var sql = SirenTools.Sql() + "WHERE  (Societe_ID = '001')" + Environment.NewLine;
                var siren = Tools.DigitOnly(q);
                object param = null;

                if (siren.Length >= 9)
                {
                    // Recherche par n° siren
                    sql += "AND    (Siren = :Siren)";
                    param = new { Siren = siren.Substring(0, 9) };
                }
                else if (siren.Length < 3)
                {
                    // Recherche par raison sociale seule
                    sql += "AND    (UPPER(Raison_Social) LIKE :Nom)";
                    param = new { Nom = "%" + q.ToUpperInvariant() + "%" };
                }
                else
                {
                    // Recherche par raison sociale ou n° siren
                    sql += "AND    ((UPPER(Raison_Social) LIKE :Nom) OR (Siren LIKE :Siren))";
                    param = new { Nom = "%" + q.ToUpperInvariant() + "%", Siren = siren + "%" };
                }
                sql += " ORDER BY UPPER(Raison_Social), Siren";
                data = connexion.Query<DbSiren>(sql, param);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbSiren>, IEnumerable<Siren>>(data);
            return view_model;
        }

        public Siren GetSiren(int id)
        {
            var data = new DbSiren();

            try
            {
                connexion.Open();
                var sql = SirenTools.Sql() + "WHERE  (ID = :Id)";
                data = connexion.Query<DbSiren>(sql, new { id }).FirstOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<Siren>(data);
            return view_model;
        }

        public IEnumerable<Garantie> GetGaranties(int Siren_ID = 0, int Client_ID = 0, int Garantie_ID = 0)
        {
            IEnumerable<DbGarantie> data = null;

            var sql = @"SELECT R.Risque_ID
                             , R.Client_ID
                             , R.Montant_Risque
                             , R.Option_Risque
                             , R.Date_Risque
                             , R.Periode_Debut
                             , R.Periode_Fin
                             , R.Montant_Risque_Compl
                             , R.Date_Debut_Risque
                             , R.Date_Fin_Risque
                             , R.Garantie_Interne
                             , R.Garantie_Periode_Debut
                             , R.Garantie_Periode_Fin
                             , R.Mnt_Oal
                             , R.Dte_Debut_Oal
                             , R.Dte_Fin_Oal
                             , R.Mnt_Cap
                             , R.Option_Cap
                             , R.Dte_Debut_Cap
                             , R.Dte_Fin_Cap
                             , R.Montant_Deblocage
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
                data = connexion.Query<DbGarantie>(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbGarantie>, IEnumerable<Garantie>>(data);
            return view_model;
        }

        public IEnumerable<Decision> GetDecisions(int Siren_ID = 0, int Decision_ID = 0)
        {
            IEnumerable<DbDecision> data = null;

            var sql = @"SELECT H.Historique_ID
                             , S.ID AS Siren_ID
                             , H.Decision_Date
                             , H.Significatif
                             , H.Result_Code
                             , H.Decision_Code
                             , H.Condition_Code
                             , H.Montant
                             , H.Second_Montant
                             , H.Date_Effet
                             , H.Date_Fin_Effet
                             , H.Date_Entree
                             , H.TCode
                             , H.Supersede
                             , H.Date_Last_Update
                             , H.Date_Import
                             , H.Date_Fichier
                        FROM   Ct_Historique_Atradius H ";

            if (Decision_ID != 0)
            {
                sql += @"    , Ct_Fiche_Siren S
                         WHERE (H.Historique_ID = {decision_id})
                         AND   (S.Siren = H.Siren)";
                sql = sql.Replace("{decision_id}", Decision_ID.ToString());
            }
            else
            {
                sql += @"    , Ct_Fiche_Siren S
                         WHERE (S.ID = {siren_id})
                         AND   (H.Siren = S.Siren)
                         ORDER BY H.Decision_Date DESC
                                , H.Date_Effet DESC
                                , H.Date_Last_Update DESC
                                , H.Historique_ID DESC";
                sql = sql.Replace("{siren_id}", Siren_ID.ToString());
            }

            try
            {
                connexion.Open();
                data = connexion.Query<DbDecision>(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbDecision>, IEnumerable<Decision>>(data);
            return view_model;
        }
    }
}
