using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using AutoMapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet EdiQualification utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class EdiQualification
    {
        public int Qualification_ID { get; set; }
        public int Site_ID { get; set; }
        public string Libelle { get; set; }
        public string Code { get; set; }
        public string Pcs { get; set; }
        public bool EstBloque { get; set; }

        public EdiSite EdiSite { get; set; }
    }

    /// <summary>
    /// Objet DbEdiQualification stocké dans la base de données
    /// </summary>
    [Table("Qualification_Edi")]
    public class DbEdiQualification
    {
        public int Id { get; set; }
        public int Etablissement_Id { get; set; }
        public string Libelle { get; set; }
        public string Code_Edi { get; set; }
        public string Code_Pcs { get; set; }
        public int Active { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les qualifications
    /// </summary>
    public class EdiQualifications
    {
        private IDbConnection connexion;

        public EdiQualifications(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public List<EdiQualification> List(int Site_ID)
        {
            var where = @"WHERE  (Etablissement_Id = :Id)
                          ORDER BY UPPER(Libelle)";

            var data = connexion.List<DbEdiQualification>(where, new { Id = Site_ID });
            var view_model = Mapper.Map<IEnumerable<DbEdiQualification>, List<EdiQualification>>(data);

            return view_model;
        }

        public EdiQualification Get(int id)
        {
            var data = this.connexion.Get<DbEdiQualification>(id);
            var view_model = Mapper.Map<EdiQualification>(data);

            return view_model;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbEdiQualification à EdiQualification
        /// </summary>
        public static void EdiQualifications()
        {
            Mapper.CreateMap<DbEdiQualification, EdiQualification>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbEdiQualification, EdiQualification>()
                .ForMember(dest => dest.Qualification_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Site_ID, opt => opt.MapFrom(src => src.Etablissement_Id))
                .ForMember(dest => dest.Libelle, opt => opt.MapFrom(src => src.Libelle))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code_Edi))
                .ForMember(dest => dest.Pcs, opt => opt.MapFrom(src => src.Code_Pcs))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => (src.Active == 0)))
            ;
        }
    }
}
