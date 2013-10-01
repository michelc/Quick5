using AutoMapper;

namespace Quick5.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Siren
            // http://stackoverflow.com/questions/954480/automapper-ignore-the-rest#8433682
            Mapper.CreateMap<DbSiren, Siren>().ForAllMembers(opt => opt.Ignore());
            Mapper.CreateMap<DbSiren, Siren>()
                .ForMember(dest => dest.Siren_ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Raison_Social))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Blocage)))
                ;

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
        }
    }
}