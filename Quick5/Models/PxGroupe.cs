using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet PxGroupe utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class PxGroupe
    {
        public int Groupe_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public string Code { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }

        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<PxSite> Sites { get; set; }
    }

    /// <summary>
    /// Objet DbPxGroupe stocké dans la base de données
    /// </summary>
    [Table("Accord_Edi_National")]
    public class DbPxGroupe
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Siren { get; set; }
        public string Code_Externe_Eu { get; set; }
        public DateTime Date_Debut { get; set; }
        public DateTime Date_Fin { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les groupes
    /// </summary>
    public class PxGroupes
    {
        private IDbConnection connexion;

        public PxGroupes(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<PxGroupe> List()
        {
            var where = @"WHERE  (Plateforme_ID = 3)
                          ORDER BY UPPER(Libelle)";

            var data = connexion.List<DbPxGroupe>(where, null);
            var view_model = Mapper.Map<IEnumerable<DbPxGroupe>, List<PxGroupe>>(data);

            return view_model;
        }

        public PxGroupe Get(int id)
        {
            var data = this.connexion.Get<DbPxGroupe>(id);
            var view_model = Mapper.Map<PxGroupe>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbPxGroupe à PxGroupe
        /// </summary>
        public static void PxGroupes()
        {
            Mapper.CreateMap<DbPxGroupe, PxGroupe>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbPxGroupe, PxGroupe>()
                .ForMember(dest => dest.Groupe_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code_Externe_Eu))
                .ForMember(dest => dest.Debut, opt => opt.MapFrom(src => src.Date_Debut))
                .ForMember(dest => dest.Fin, opt => opt.MapFrom(src => src.Date_Fin))
            ;
        }
    }
}
