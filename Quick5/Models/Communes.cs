using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;
using Quick5.Helpers;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Commune utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Commune
    {
        public int Commune_ID { get; set; }
        public string Nom { get; set; }
        public string Insee { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool EstBloque { get; set; }
    }

    /// <summary>
    /// Objet DbCommune stocké dans la base de données
    /// </summary>
    [Table("Ref_Code_Insee_Commune")]
    public class DbCommune
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public string Code_Insee { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Is_Actif { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les communes
    /// </summary>
    public class Communes
    {
        private IDbConnection connexion;

        public Communes(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<Commune> List(string q)
        {
            q = q.Trim().ToUpperInvariant();
            if (q.Length < 3) return new List<Commune>();

            var insee = Tools.DigitOnly(q);
            var where = "";
            object param = null;

            if (q.StartsWith("#"))
            {
                // Recherche par ID commune
                where += "WHERE  (Id = :Id)";
                param = new { Id = Convert.ToInt64(insee) };
            }
            else if (insee.Length == 5)
            {
                // Recherche par code insee
                where += "WHERE  (Code_Insee = :Insee)";
                param = new { insee };
            }
            else if (((q.StartsWith("2A") || q.StartsWith("2B")) && (q.Length == 5)))
            {
                // Recherche par code insee corse
                where += "WHERE  (Code_Insee = :Insee)";
                param = new { Insee = q };
            }
            else
            {
                // Recherche par nom commune
                where += "WHERE  (UPPER(Libelle) LIKE :Nom)";
                param = new { Nom = q + "%" };
            }
            where += Environment.NewLine;
            where += "ORDER BY UPPER(Libelle), Code_Insee";

            var data = connexion.List<DbCommune>(where, param);
            var view_model = Mapper.Map<IEnumerable<DbCommune>, List<Commune>>(data);

            return view_model;
        }

        public Commune Get(int id)
        {
            var data = this.connexion.Get<DbCommune>(id);
            var view_model = Mapper.Map<Commune>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbCommune à Commune
        /// </summary>
        public static void Communes()
        {
            Mapper.CreateMap<DbCommune, Commune>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbCommune, Commune>()
                .ForMember(dest => dest.Commune_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.Insee, opt => opt.MapFrom(src => src.Code_Insee))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => src.Is_Actif == 0))
            ;
        }
    }
}
