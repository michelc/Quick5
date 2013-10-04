using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;
using Quick5.Helpers;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Siren utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Siren
    {
        public int Siren_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public bool EstBloque { get; set; }

        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Garantie> Garanties { get; set; }
        public IEnumerable<Decision> Decisions { get; set; }
    }

    /// <summary>
    /// Objet DbSiren stocké dans la base de données
    /// </summary>
    public class DbSiren
    {
        public int ID { get; set; }
        public string Raison_Social { get; set; }
        public string Siren { get; set; }
        public string Blocage { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les sirens
    /// </summary>
    public class Sirens
    {
        private IDbConnection connexion;

        public Sirens(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public IEnumerable<Siren> List(string q)
        {
            IEnumerable<DbSiren> data = null;

            try
            {
                connexion.Open();

                var sql = Sql() + "WHERE  (Societe_ID = '001')" + Environment.NewLine;
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

        public Siren Get(int id)
        {
            var data = new DbSiren();

            try
            {
                connexion.Open();
                var sql = Sql() + "WHERE  (ID = :Id)";
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

        /// <summary>
        /// Requête SQL pour attaquer la table des Sirens
        /// </summary>
        /// <returns></returns>
        private string Sql()
        {
            var sql = @"SELECT ID
                             , Raison_Social
                             , Siren
                             , Blocage
                        FROM   Ct_Fiche_Siren
                        ";

            return sql;
        }
    }

    public static partial class AutoMapConfigure
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbSiren à Siren
        /// </summary>
        public static void Sirens()
        {
            // http://stackoverflow.com/questions/954480/automapper-ignore-the-rest#8433682
            Mapper.CreateMap<DbSiren, Siren>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbSiren, Siren>()
                .ForMember(dest => dest.Siren_ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Raison_Social))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Blocage)))
                ;
        }
    }
}
