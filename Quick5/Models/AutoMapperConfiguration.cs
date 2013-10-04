using AutoMapper;

namespace Quick5.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapConfigure.Sirens();

            // Client
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

            // Garantie
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
                .ForMember(dest => dest.CplFin, opt => opt.MapFrom(src => src.Date_Debut_Risque))
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

            // Décision
            Mapper.CreateMap<DbDecision, Decision>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbDecision, Decision>()
                .ForMember(dest => dest.Decision_ID, opt => opt.MapFrom(src => src.Historique_ID))
                .ForMember(dest => dest.Siren_ID, opt => opt.MapFrom(src => src.ID))
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