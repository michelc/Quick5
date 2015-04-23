using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet PxSite utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class PxSite
    {
        public int Site_ID { get; set; }
        public int Groupe_ID { get; set; }
        public string Nom { get; set; }
        public string NSiret { get; set; }
        public string Code { get; set; }

        public PxGroupe PxGroupe { get; set; }
    }

    /// <summary>
    /// Objet DbPxSite stocké dans la base de données
    /// </summary>
    [Table("Etablissement_Edi")]
    public class DbPxSite
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
    public class PxSites
    {
        private IDbConnection connexion;

        public PxSites(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<PxSite> List(int Groupe_ID)
        {
            var where = @"WHERE  (Accord_National_Id = :Id)
                          ORDER BY UPPER(Libelle)";

            var data = connexion.List<DbPxSite>(where, new { Id = Groupe_ID });
            var view_model = Mapper.Map<IEnumerable<DbPxSite>, List<PxSite>>(data);

            return view_model;
        }

        public PxSite Get(int id)
        {
            var data = this.connexion.Get<DbPxSite>(id);
            var view_model = Mapper.Map<PxSite>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbPxSite à PxSite
        /// </summary>
        public static void PxSites()
        {
            Mapper.CreateMap<DbPxSite, PxSite>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbPxSite, PxSite>()
                .ForMember(dest => dest.Site_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Groupe_ID, opt => opt.MapFrom(src => src.Accord_National_Id))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.NSiret, opt => opt.MapFrom(src => src.Siret))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code_Externe_Eu))
            ;
        }
    }
}
