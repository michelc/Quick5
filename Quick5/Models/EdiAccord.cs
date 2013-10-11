using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet EdiAccord utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class EdiAccord
    {
        public int Accord_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public string Code { get; set; }

        public IEnumerable<EdiSite> Sites { get; set; }
    }

    /// <summary>
    /// Objet DbEdiAccord stocké dans la base de données
    /// </summary>
    [Table("Accord_Edi_National")]
    public class DbEdiAccord
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Siren { get; set; }
        public string Code_Externe_Eu { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les accords
    /// </summary>
    public class EdiAccords
    {
        private IDbConnection connexion;

        public EdiAccords(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public IEnumerable<EdiAccord> List()
        {
            var where = "ORDER BY UPPER(Libelle)";

            var data = connexion.List<DbEdiAccord>(where, null);
            var view_model = Mapper.Map<IEnumerable<DbEdiAccord>, IEnumerable<EdiAccord>>(data);

            return view_model;
        }

        public EdiAccord Get(int id)
        {
            var data = this.connexion.Get<DbEdiAccord>(id);
            var view_model = Mapper.Map<EdiAccord>(data);

            return view_model;
        }

        /// <summary>
        /// Requête SQL pour attaquer la table des accords edi
        /// </summary>
        /// <returns></returns>
        private string Sql()
        {
            var sql = @"SELECT Id
                             , Libelle
                             , Siren
                             , Code_Externe_Eu
                        FROM   Accord_Edi_National
                        ";

            return sql;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbEdiAccord à EdiAccord
        /// </summary>
        public static void EdiAccords()
        {
            Mapper.CreateMap<DbEdiAccord, EdiAccord>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbEdiAccord, EdiAccord>()
                .ForMember(dest => dest.Accord_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code_Externe_Eu))
            ;
        }
    }
}
