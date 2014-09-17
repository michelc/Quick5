using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;
using Quick5.Helpers;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Insee utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Insee
    {
        public string ID { get; set; }
        public string Nom { get; set; }
    }

    /// <summary>
    /// Objet DbInsee stocké dans la base de données
    /// </summary>
    [Table("Mcr_Insees")]
    public class DbInsee
    {
        public string ID { get; set; }
        public string Nom { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les communes insee
    /// </summary>
    public class Insees
    {
        private IDbConnection connexion;

        public Insees(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<Insee> List(string q)
        {
            q = q.Trim().ToUpperInvariant();
            if (q.Length < 3) return new List<Insee>();

            var insee = Tools.DigitOnly(q);
            var where = "";
            object param = null;

            if (q.StartsWith("#"))
            {
                // Recherche par ID commune
                where += "WHERE  (Id = :Id)";
                param = new { Id = Convert.ToInt64(insee) };
            }
            else
            {
                // Recherche par nom commune
                where += "WHERE  (Nom LIKE :Nom)";
                param = new { Nom = q + "%" };
            }
            where += Environment.NewLine;
            where += "ORDER BY Nom, ID";

            var data = connexion.List<DbInsee>(where, param);
            var view_model = Mapper.Map<IEnumerable<DbInsee>, List<Insee>>(data);

            return view_model;
        }

        public Insee Get(string id)
        {
            var data = this.connexion.Get<DbInsee>(id);
            var view_model = Mapper.Map<Insee>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbInsee à Insee
        /// </summary>
        public static void Insees()
        {
            Mapper.CreateMap<DbInsee, Insee>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbInsee, Insee>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom))
            ;
        }
    }
}
