using AutoMapper;

namespace membresias.be.Models.Dtos
{
    public class FallecimientoDto
    {
        public int IdFallecimiento { get; set; }
        public int IdMiembro { get; set; }
        public string FechaFallecimiento { get; set; }
    }

    public class FallecimientoProfile : Profile
    {
        public FallecimientoProfile()
        {
            CreateMap<Fallecimiento, FallecimientoDto>()
                .ForMember(dest => dest.FechaFallecimiento, 
                opt => opt.MapFrom(src => src.FechaFallecimiento.Date.ToString("yyyy-MM-dd"))); 
        }
    }
}
