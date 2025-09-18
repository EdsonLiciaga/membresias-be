using AutoMapper;
using membresias.be.Enumerations;

namespace membresias.be.Models.Dtos
{
    public class MiembroDto
    {
        public int IdMiembro { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string Clave { get; set; }
        public string SegundoApellido { get; set; }
        public string Curp { get; set; }
        public string FechaNacimiento { get; set; }
        public int edad { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaReingreso { get; set; }
        public string FechaFallecimiento { get; set; } = string.Empty; 
        public string MembresiaCodigo { get; set; }
        public string MembresiaNombre { get; set; }
        public string MiembroEstatusCodigo { get; set; }
        public string MiembroEstatusNombre { get; set; }
    }

    public class MiembroProfile : Profile
    {
        public MiembroProfile()
        {
            CreateMap<Miembro, MiembroDto>()
                .ForMember(dest => dest.FechaNacimiento,
                    opt => opt.MapFrom(src => src.FechaNacimiento.Date.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.FechaIngreso, 
                    opt => opt.MapFrom(src => src.FechaIngreso.Date.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.FechaReingreso,
                    opt => opt.MapFrom((src, _) => src.FechaReingreso?.Date.ToString("yyyy-MM-dd") ?? string.Empty))
                .ForMember(dest => dest.MembresiaNombre,
                    opt => opt.MapFrom(src => Membresia.GetByCode(src.MembresiaCodigo).Nombre))
                .ForMember(dest => dest.MiembroEstatusNombre,
                    opt => opt.MapFrom(src => MiembroEstatus.GetByCode(src.MiembroEstatusCodigo).Nombre));  
        }
    }
}
