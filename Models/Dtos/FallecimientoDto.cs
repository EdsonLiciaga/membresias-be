using AutoMapper;

namespace membresias.be.Models.Dtos
{
    public class FallecimientoDto
    {
        public int IdFallecimiento { get; set; }
        public string FechaFallecimiento { get; set; }
    }

    public class FallecimientoProfile : Profile
    {
        public FallecimientoProfile()
        {
            CreateMap<Fallecimiento, FallecimientoDto>(); 
        }
    }
}
