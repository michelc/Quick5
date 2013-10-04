using System.Collections.Generic;
using AutoMapper;

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
    public static class SirenTools
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbSiren à Siren
        /// </summary>
        public static void AutoMap()
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

        /// <summary>
        /// Requête SQL pour attaquer la table des Sirens
        /// </summary>
        /// <returns></returns>
        public static string Sql()
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
}
