using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet MdxOrganisation utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class MdxOrganisation
    {
        public string ID { get; set; }
        public string Libelle { get; set; }
        public string Parent_ID { get; set; }
    }

    /// <summary>
    /// Objet DbEdiQualification stocké dans la base de données
    /// </summary>
    [Table("Mdx_Organisations")]
    public class DbMdxOrganisation
    {
        public string ID { get; set; }
        public string Libelle { get; set; }
        public string Parent_ID { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les organisations
    /// </summary>
    public class MdxOrganisations
    {
        private IDbConnection connexion;

        public MdxOrganisations(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<MdxOrganisation> List()
        {
            var where = @"ORDER BY Libelle";

            var data = connexion.List<DbMdxOrganisation>(where, null);
            var view_model = Mapper.Map<IEnumerable<DbMdxOrganisation>, List<MdxOrganisation>>(data);

            return view_model;
        }

        public MdxOrganisation Get(string id)
        {
            var data = this.connexion.Get<DbMdxOrganisation>(id);
            var view_model = Mapper.Map<MdxOrganisation>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbMdxOrganisation à MdxOrganisation
        /// </summary>
        public static void MdxOrganisations()
        {
            Mapper.CreateMap<DbMdxOrganisation, MdxOrganisation>();
        }
    }
}
