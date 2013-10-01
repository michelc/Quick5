using AutoMapper;

namespace Quick5.Models
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // Siren
            Mapper.CreateMap<DbSiren, Siren>()
                .ForMember(dest => dest.Siren_ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Raison_Social))
                .ForMember(dest => dest.NSiren, opt => opt.MapFrom(src => src.Siren))
                .ForMember(dest => dest.EstBloque, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Blocage)))
                ;
        }
    }
}