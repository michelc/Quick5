using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using AutoMapper;
using Dapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Decision utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Decision
    {
        public int Decision_ID { get; set; }
        public DateTime DDecision { get; set; }
        public bool Significatif { get; set; }
        public string Resultat { get; set; }
        public string Code { get; set; }
        public string Condition { get; set; }
        public decimal Montant { get; set; }
        public decimal Complement { get; set; }
        public DateTime Debut { get; set; }
        public DateTime Fin { get; set; }
        public DateTime DEntree { get; set; }
        public string TCode { get; set; }
        public bool Super { get; set; }
        public DateTime DUpdate { get; set; }
        public DateTime DImport { get; set; }
        public DateTime DFichier { get; set; }
    }

    /// <summary>
    /// Objet DbDecision stocké dans la base de données
    /// </summary>
    [Table("Ct_Historique_Atradius")]
    public class DbDecision
    {
        public int Historique_ID { get; set; }
        public string Siren { get; set; }
        public DateTime Decision_Date { get; set; }
        public string Significatif { get; set; }
        public string Result_Code { get; set; }
        public string Decision_Code { get; set; }
        public string Condition_Code { get; set; }
        public decimal Montant { get; set; }
        public decimal Second_Montant { get; set; }
        public DateTime Date_Effet { get; set; }
        public DateTime Date_Fin_Effet { get; set; }
        public DateTime Date_Entree { get; set; }
        public string TCode { get; set; }
        public int Supersede { get; set; }
        public DateTime Date_Last_Update { get; set; }
        public DateTime Date_Import { get; set; }
        public DateTime Date_Fichier { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les décisions
    /// </summary>
    public class Decisions
    {
        private IDbConnection connexion;

        public Decisions(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public IEnumerable<Decision> List(string NSiren)
        {
            IEnumerable<DbDecision> data = null;

            try
            {
                connexion.Open();
                var sql = Sql();
                sql += @"WHERE (Siren = :NSiren)
                         ORDER BY Decision_Date DESC
                                , Date_Effet DESC
                                , Date_Last_Update DESC
                                , Historique_ID DESC";

                data = connexion.Query<DbDecision>(sql, new { NSiren });
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            var view_model = Mapper.Map<IEnumerable<DbDecision>, IEnumerable<Decision>>(data);
            return view_model;
        }

        /// <summary>
        /// Requête SQL pour attaquer la table des décisions
        /// </summary>
        /// <returns></returns>
        private string Sql()
        {
            var sql = @"SELECT Historique_ID
                            , Siren
                            , Decision_Date
                            , Significatif
                            , Result_Code
                            , Decision_Code
                            , Condition_Code
                            , Montant
                            , Second_Montant
                            , Date_Effet
                            , Date_Fin_Effet
                            , Date_Entree
                            , TCode
                            , Supersede
                            , Date_Last_Update
                            , Date_Import
                            , Date_Fichier
                        FROM   Ct_Historique_Atradius
                        ";

            return sql;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbDecision à Decision
        /// </summary>
        public static void Decisions()
        {
            Mapper.CreateMap<DbDecision, Decision>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbDecision, Decision>()
                .ForMember(dest => dest.Decision_ID, opt => opt.MapFrom(src => src.Historique_ID))
                .ForMember(dest => dest.DDecision, opt => opt.MapFrom(src => src.Decision_Date))
                .ForMember(dest => dest.Significatif, opt => opt.MapFrom(src => src.Significatif != "0"))
                .ForMember(dest => dest.Resultat, opt => opt.MapFrom(src => src.Result_Code))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Decision_Code))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition_Code))
                .ForMember(dest => dest.Montant, opt => opt.MapFrom(src => src.Montant))
                .ForMember(dest => dest.Complement, opt => opt.MapFrom(src => src.Second_Montant))
                .ForMember(dest => dest.Debut, opt => opt.MapFrom(src => src.Date_Effet))
                .ForMember(dest => dest.Fin, opt => opt.MapFrom(src => src.Date_Fin_Effet))
                .ForMember(dest => dest.DEntree, opt => opt.MapFrom(src => src.Date_Entree))
                .ForMember(dest => dest.TCode, opt => opt.MapFrom(src => src.TCode))
                .ForMember(dest => dest.Super, opt => opt.MapFrom(src => src.Supersede != 0))
                .ForMember(dest => dest.DUpdate, opt => opt.MapFrom(src => src.Date_Last_Update))
                .ForMember(dest => dest.DImport, opt => opt.MapFrom(src => src.Date_Import))
                .ForMember(dest => dest.DFichier, opt => opt.MapFrom(src => src.Date_Fichier))
                ;
        }
    }
}
