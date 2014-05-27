using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using AutoMapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Agence utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Agence
    {
        public int Agence_ID { get; set; }
        public string Nom { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Siret { get; set; }
        public string Urssaf { get; set; }
        public string Siret_Urssaf { get; set; }
        public int Rattachement_ID { get; set; }
    }

    /// <summary>
    /// Objet DbAgence stocké dans la base de données
    /// </summary>
    [Table("V_Agences")]
    public class DbAgence
    {
        public string Code_Agn { get; set; }
        public string Societe_ID { get; set; }
        public string Libelle { get; set; }
        public string Adresse { get; set; }
        public string Adresse2 { get; set; }
        public string Departement { get; set; }
        public string Ville { get; set; }
        public string Siret { get; set; }
        public string Urssaf { get; set; }
        public string Siret_Urssaf { get; set; }
        public string Ag_Rattachement { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les agences
    /// </summary>
    public class Agences
    {
        private IDbConnection connexion;

        public Agences(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<Agence> List()
        {
            // Retrouve uniquement les agences avec de l'activité au cours des 2 derniers mois
            var where = @"ORDER BY Libelle";

            var data = connexion.List<DbAgence>(where, null);
            var view_model = Mapper.Map<IEnumerable<DbAgence>, List<Agence>>(data);

            return view_model;
        }

        public Agence Get(int id)
        {
            var where = "WHERE  (Code_Agn = :Id)";

            var data = connexion.List<DbAgence>(where, new { Id = id.ToString("000") }).FirstOrDefault();
            var view_model = Mapper.Map<Agence>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbAgence à Agence
        /// </summary>
        public static void Agences()
        {
            Mapper.CreateMap<DbAgence, Agence>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbAgence, Agence>()
                .ForMember(dest => dest.Agence_ID, opt => opt.MapFrom(src => src.Code_Agn))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.Adresse1, opt => opt.MapFrom(src => src.Adresse))
                .ForMember(dest => dest.Adresse2, opt => opt.MapFrom(src => src.Adresse2))
                .ForMember(dest => dest.CodePostal, opt => opt.MapFrom(src => src.Departement))
                .ForMember(dest => dest.Ville, opt => opt.MapFrom(src => src.Ville))
                .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.Siret))
                .ForMember(dest => dest.Urssaf, opt => opt.MapFrom(src => src.Urssaf))
                .ForMember(dest => dest.Siret_Urssaf, opt => opt.MapFrom(src => src.Siret_Urssaf))
                .ForMember(dest => dest.Rattachement_ID, opt => opt.MapFrom(src => src.Ag_Rattachement))
            ;
        }
    }
}
