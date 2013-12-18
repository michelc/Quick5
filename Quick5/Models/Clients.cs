using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;
using Quick5.Helpers;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Client utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Client
    {
        public int Client_ID { get; set; }
        public string Nom { get; set; }
        public string NSiren { get; set; }
        public string NSiret { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Type { get; set; }
        public bool EstBloque { get; set; }

        public Siren Siren { get; set; }
        public Garantie Garantie { get; set; }
    }

    /// <summary>
    /// Objet DbClient stocké dans la base de données
    /// </summary>
    [Table("Cy")]
    public class DbClient
    {
        public int IdCompany { get; set; }
        public string Name { get; set; }
        public string Siren { get; set; }
        public string Fld109 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Fld138 { get; set; }
        public string Fld129 { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les clients
    /// </summary>
    public class Clients
    {
        private IDbConnection connexion;

        public Clients(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<Client> List(string q)
        {
            var where = "";
            object param = null;

            q = q.Trim().ToUpperInvariant();
            var siren = Tools.DigitOnly(q);
            if (q.StartsWith("#"))
            {
                // Recherche par ID client
                where += "WHERE  (IdCompany = :Id)";
                param = new { Id = Convert.ToInt64(siren) };
            }
            else if (siren.Length >= 14)
            {
                // Recherche par n° siret
                where += "WHERE  (Fld109 = :Siret)";
                param = new { Siret = siren.Substring(0, 14) };
            }
            else if (siren.Length >= 9)
            {
                // Recherche par n° siren
                where += "WHERE  (Siren = :Siren)";
                param = new { Siren = siren.Substring(0, 9) };
            }
            else if (siren.Length < 3)
            {
                // Recherche par nom client seul
                where += "WHERE  (UPPER(Name) LIKE :Nom)";
                param = new { Nom = q + "%" };
            }
            else
            {
                // Recherche par nom client ou n° siren
                where += "WHERE  ((UPPER(Name) LIKE :Nom) OR (Siren LIKE :Siren))";
                param = new { Nom = q + "%", Siren = q + "%" };
            }
            where += Environment.NewLine;
            where += "ORDER BY UPPER(Name), UPPER(City)";

            var data = connexion.List<DbClient>(where, param);
            var view_model = Mapper.Map<IEnumerable<DbClient>, List<Client>>(data);

            return view_model;
        }

        public Client Get(int id)
        {
            var data = this.connexion.Get<DbClient>(id);
            var view_model = Mapper.Map<Client>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbClient à Client
        /// </summary>
        public static void Clients()
        {
            Mapper.CreateMap<DbClient, Client>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbClient, Client>()
                .ForMember(dest => dest.Client_ID, opt => opt.MapFrom(src => src.IdCompany))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.NSiret, opt => opt.MapFrom(src => src.Fld109))
                .ForMember(dest => dest.CodePostal, opt => opt.MapFrom(src => src.PostCode))
                .ForMember(dest => dest.Ville, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Fld138))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Fld129)))
            ;
        }
    }
}
