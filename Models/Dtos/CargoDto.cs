using AutoMapper;
using membresias.be.Enumerations;

namespace membresias.be.Models.Dtos
{
    public class CargoDto
    {
        public int IdCargo { get; set; }
        public string Descripcion { get; set; }
        public int Monto { get; set; }
        public string FechaCargo { get; set; }
        public bool IsPagado { get; set; }
        public int IdMiembro { get; set; }
        public string ConceptoCodigo { get; set; }
        public string ConceptoNombre { get; set; } 
    }

    public class CargoProfile : Profile
    {
        public CargoProfile()
        {
            CreateMap<Cargo, CargoDto>()
                .ForMember(dest => dest.FechaCargo,
                    opt => opt.MapFrom(src => src.FechaCargo.Date.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.ConceptoNombre, 
                    opt => opt.MapFrom(src => Concepto.GetByCode(src.ConceptoCodigo).Nombre))
                .ForMember(dest => dest.Descripcion, 
                    opt => opt.MapFrom(src => src.Descripcion ?? string.Empty)); 
        }
    }
}
