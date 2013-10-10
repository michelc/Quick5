using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet EdiSite utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class EdiSite
    {
        public int Site_ID { get; set; }
        public int Accord_ID { get; set; }
        public string Nom { get; set; }
        public string NSiret { get; set; }
        public string Code { get; set; }
    }

    /// <summary>
    /// Objet DbEdiSite stocké dans la base de données
    /// </summary>
    [Table("Etablissement_Edi")]
    public class DbEdiSite
    {
        public int Id { get; set; }
        public int Accord_National_Id { get; set; }
        public string Libelle { get; set; }
        public string Siret { get; set; }
        public string Code_Externe_Eu { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les accords
    /// </summary>
    public class EdiSites
    {
        private IDbConnection connexion;

        public EdiSites(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public IEnumerable<EdiSite> List(int Accord_ID)
        {
            IEnumerable<DbEdiSite> data = null;

            try
            {
                connexion.Open();

                var sql = Sql() + "WHERE  (Accord_National_Id = :Id)";
                data = connexion.Query<DbEdiSite>(sql, new { id = Accord_ID }).OrderBy(a => a.Libelle.ToUpperInvariant());
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbEdiSite>, IEnumerable<EdiSite>>(data);
            return view_model;
        }

        public EdiSite Get(int id)
        {
            var data = new DbEdiSite();

            try
            {
                connexion.Open();
                var sql = Sql() + "WHERE  (Id = :Id)";
                data = connexion.Query<DbEdiSite>(sql, new { id }).FirstOrDefault();
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<EdiSite>(data);
            return view_model;
        }

        /// <summary>
        /// Requête SQL pour attaquer la table des accords edi
        /// </summary>
        /// <returns></returns>
        private string Sql()
        {
            var sql = @"SELECT Id
                             , Accord_National_Id
                             , Libelle
                             , Siret
                             , Code_Externe_Eu
                        FROM   Etablissement_Edi
                        ";

            return sql;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbEdiSite à EdiSite
        /// </summary>
        public static void EdiSites()
        {
            Mapper.CreateMap<DbEdiSite, EdiSite>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbEdiSite, EdiSite>()
                .ForMember(dest => dest.Site_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Accord_ID, opt => opt.MapFrom(src => src.Accord_National_Id))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.NSiret, opt => opt.MapFrom(src => src.Siret))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code_Externe_Eu))
            ;
        }
    }
}
