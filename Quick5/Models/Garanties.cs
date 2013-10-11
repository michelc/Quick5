using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using AutoMapper;
using Dapper;

namespace Quick5.Models
{
    /// <summary>
    /// Objet Garantie utilisé par l'application (essentiellement dans les vues)
    /// </summary>
    public class Garantie
    {
        public int Garantie_ID { get; set; }
        public int Client_ID { get; set; }
        // Garantie principale
        public decimal GarMontant { get; set; }
        public string GarOption { get; set; }
        public DateTime? GarDate { get; set; }
        public DateTime? GarDebut { get; set; }
        public DateTime? GarFin { get; set; }
        // Garantie complémentaire
        public decimal CplMontant { get; set; }
        public DateTime? CplDebut { get; set; }
        public DateTime? CplFin { get; set; }
        // Garantie interne
        public decimal IntMontant { get; set; }
        public DateTime? IntDebut { get; set; }
        public DateTime? IntFin { get; set; }
        // Garantie OAL
        public decimal OalMontant { get; set; }
        public DateTime? OalDebut { get; set; }
        public DateTime? OalFin { get; set; }
        // Garantie CAP
        public decimal CapMontant { get; set; }
        public string CapOption { get; set; }
        public DateTime? CapDebut { get; set; }
        public DateTime? CapFin { get; set; }
        // Montant déblocage
        public decimal Deblocage { get; set; }
        // Garantie totale
        public decimal Totale { get; set; }

        public Client Client { get; set; }
        public Siren Siren { get; set; }
    }

    /// <summary>
    /// Objet DbGarantie stocké dans la base de données
    /// </summary>
    [Table("Ct_Risques_Clients")]
    public class DbGarantie
    {
        public int Risque_ID { get; set; }
        public int Client_ID { get; set; }
        // Garantie principale
        public decimal Montant_Risque { get; set; }
        public string Option_Risque { get; set; }
        public DateTime? Date_Risque { get; set; }
        public DateTime? Periode_Debut { get; set; }
        public DateTime? Periode_Fin { get; set; }
        // Garantie complémentaire
        public decimal Montant_Risque_Compl { get; set; }
        public DateTime? Date_Debut_Risque { get; set; }
        public DateTime? Date_Fin_Risque { get; set; }
        // Garantie interne
        public decimal Garantie_Interne { get; set; }
        public DateTime? Garantie_Periode_Debut { get; set; }
        public DateTime? Garantie_Periode_Fin { get; set; }
        // Garantie OAL
        public decimal Mnt_Oal { get; set; }
        public DateTime? Dte_Debut_Oal { get; set; }
        public DateTime? Dte_Fin_Oal { get; set; }
        // Garantie CAP
        public decimal Mnt_Cap { get; set; }
        public string Option_Cap { get; set; }
        public DateTime? Dte_Debut_Cap { get; set; }
        public DateTime? Dte_Fin_Cap { get; set; }
        // Montant déblocage
        public decimal Montant_Deblocage { get; set; }
    }

    /// <summary>
    /// Fonctions utilitaires pour gérer les garanties
    /// </summary>
    public class Garanties
    {
        private IDbConnection connexion;

        public Garanties(IDbConnection connexion)
        {
            this.connexion = connexion;
        }

        public IEnumerable<Garantie> List(object id)
        {
            var where = "";
            object param = null;

            if (id.GetType().Name == "String")
            {
                // Recherche par n° siren
                where += @"     , Cy
                           WHERE  (Siren = :Siren)
                           AND    (Client_ID = IdCompany)
                           ORDER BY UPPER(Name), UPPER(City)";
                param = new { Siren = (string)id };
            }
            else
            {
                // Recherche par ID client
                where += "WHERE  (Client_ID = :Client_ID)";
                param = new { Client_ID = (int)id };
            }

            var data = connexion.List<DbGarantie>(where, param);
            var view_model = Mapper.Map<IEnumerable<DbGarantie>, IEnumerable<Garantie>>(data);

            return view_model;
        }

        public Garantie Get(int id)
        {
            var data = this.connexion.Get<DbGarantie>(id);
            var view_model = Mapper.Map<Garantie>(data);

            return view_model;
        }

        public int Save(Garantie model)
        {
            var result = 0;
            var data = Mapper.Map<DbGarantie>(model);

            var sql = "UPDATE Ct_Risques_Clients SET ";
            var columns = typeof(DbGarantie).GetProperties().ToArray().Select(p => p.Name);
            var cols = columns.Skip(1).Select(c => c + " = :" + c + Environment.NewLine);
            sql += string.Join(", ", columns.Select(c => c + " = :" + c));
            sql += " WHERE  (Risque_ID = :Risque_ID)";

            try
            {
                connexion.Open();
                result = connexion.Execute(sql, data);
            }
            catch
            {
                throw;
            }
            finally
            {
                connexion.Close();
            }

            return result;
        }

        /// <summary>
        /// Requête SQL pour attaquer la table des garanties
        /// </summary>
        /// <returns></returns>
        private string Sql()
        {
            var sql = @"SELECT Risque_ID
                             , Client_ID
                             , Montant_Risque
                             , Option_Risque
                             , Date_Risque
                             , Periode_Debut
                             , Periode_Fin
                             , Montant_Risque_Compl
                             , Date_Debut_Risque
                             , Date_Fin_Risque
                             , Garantie_Interne
                             , Garantie_Periode_Debut
                             , Garantie_Periode_Fin
                             , Mnt_Oal
                             , Dte_Debut_Oal
                             , Dte_Fin_Oal
                             , Mnt_Cap
                             , Option_Cap
                             , Dte_Debut_Cap
                             , Dte_Fin_Cap
                             , Montant_Deblocage
                        FROM   Ct_Risques_Clients
                        ";

            return sql;
        }
    }

    public partial class MappingConfig
    {
        /// <summary>
        /// Configuration AutoMapper pour passer de DbGarantie à Garantie
        /// </summary>
        public static void Garanties()
        {
            Mapper.CreateMap<DbGarantie, Garantie>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbGarantie, Garantie>()
                .ForMember(dest => dest.Garantie_ID, opt => opt.MapFrom(src => src.Risque_ID))
                .ForMember(dest => dest.Client_ID, opt => opt.MapFrom(src => src.Client_ID))
                .ForMember(dest => dest.GarMontant, opt => opt.MapFrom(src => src.Montant_Risque))
                .ForMember(dest => dest.GarOption, opt => opt.MapFrom(src => src.Option_Risque))
                .ForMember(dest => dest.GarDate, opt => opt.MapFrom(src => src.Date_Risque))
                .ForMember(dest => dest.GarDebut, opt => opt.MapFrom(src => src.Periode_Debut))
                .ForMember(dest => dest.GarFin, opt => opt.MapFrom(src => src.Periode_Fin))
                .ForMember(dest => dest.CplMontant, opt => opt.MapFrom(src => src.Montant_Risque_Compl))
                .ForMember(dest => dest.CplDebut, opt => opt.MapFrom(src => src.Date_Debut_Risque))
                .ForMember(dest => dest.CplFin, opt => opt.MapFrom(src => src.Date_Fin_Risque))
                .ForMember(dest => dest.IntMontant, opt => opt.MapFrom(src => src.Garantie_Interne))
                .ForMember(dest => dest.IntDebut, opt => opt.MapFrom(src => src.Garantie_Periode_Debut))
                .ForMember(dest => dest.IntFin, opt => opt.MapFrom(src => src.Garantie_Periode_Fin))
                .ForMember(dest => dest.OalMontant, opt => opt.MapFrom(src => src.Mnt_Oal))
                .ForMember(dest => dest.OalDebut, opt => opt.MapFrom(src => src.Dte_Debut_Oal))
                .ForMember(dest => dest.OalFin, opt => opt.MapFrom(src => src.Dte_Fin_Oal))
                .ForMember(dest => dest.CapMontant, opt => opt.MapFrom(src => src.Mnt_Cap))
                .ForMember(dest => dest.CapOption, opt => opt.MapFrom(src => src.Option_Cap))
                .ForMember(dest => dest.CapDebut, opt => opt.MapFrom(src => src.Dte_Debut_Cap))
                .ForMember(dest => dest.CapFin, opt => opt.MapFrom(src => src.Dte_Fin_Cap))
                .ForMember(dest => dest.Deblocage, opt => opt.MapFrom(src => src.Montant_Deblocage))
                .ForMember(dest => dest.Totale, opt => opt.MapFrom(src => src.Montant_Risque
                                                                        + src.Montant_Risque_Compl
                                                                        + src.Garantie_Interne
                                                                        + src.Mnt_Oal
                                                                        + src.Mnt_Cap
                                                                        + src.Montant_Deblocage))
                ;

            Mapper.CreateMap<Garantie, DbGarantie>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<Garantie, DbGarantie>()
                .ForMember(dest => dest.Risque_ID, opt => opt.MapFrom(src => src.Garantie_ID))
                .ForMember(dest => dest.Client_ID, opt => opt.MapFrom(src => src.Client_ID))
                .ForMember(dest => dest.Montant_Risque, opt => opt.MapFrom(src => src.GarMontant))
                .ForMember(dest => dest.Option_Risque, opt => opt.MapFrom(src => src.GarOption))
                .ForMember(dest => dest.Date_Risque, opt => opt.MapFrom(src => src.GarDate))
                .ForMember(dest => dest.Periode_Debut, opt => opt.MapFrom(src => src.GarDebut))
                .ForMember(dest => dest.Periode_Fin, opt => opt.MapFrom(src => src.GarFin))
                .ForMember(dest => dest.Montant_Risque_Compl, opt => opt.MapFrom(src => src.CplMontant))
                .ForMember(dest => dest.Date_Debut_Risque, opt => opt.MapFrom(src => src.CplDebut))
                .ForMember(dest => dest.Date_Debut_Risque, opt => opt.MapFrom(src => src.CplFin))
                .ForMember(dest => dest.Garantie_Interne, opt => opt.MapFrom(src => src.IntMontant))
                .ForMember(dest => dest.Garantie_Periode_Debut, opt => opt.MapFrom(src => src.IntDebut))
                .ForMember(dest => dest.Garantie_Periode_Fin, opt => opt.MapFrom(src => src.IntFin))
                .ForMember(dest => dest.Mnt_Oal, opt => opt.MapFrom(src => src.OalMontant))
                .ForMember(dest => dest.Dte_Debut_Oal, opt => opt.MapFrom(src => src.OalDebut))
                .ForMember(dest => dest.Dte_Fin_Oal, opt => opt.MapFrom(src => src.OalFin))
                .ForMember(dest => dest.Mnt_Cap, opt => opt.MapFrom(src => src.CapMontant))
                .ForMember(dest => dest.Option_Cap, opt => opt.MapFrom(src => src.CapOption))
                .ForMember(dest => dest.Dte_Debut_Cap, opt => opt.MapFrom(src => src.CapDebut))
                .ForMember(dest => dest.Dte_Fin_Cap, opt => opt.MapFrom(src => src.CapFin))
                .ForMember(dest => dest.Montant_Deblocage, opt => opt.MapFrom(src => src.Deblocage))
                ;
        }
    }
}
