using AutoMapper;

namespace membresias.be.Models.Dtos
{
    public class EstadoCuentaDto
    {
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Clave { get; set; }
        public string MembresiaCodigo { get; set; }
        public string MembresiaNombre { get; set; }
        public string FechaEmision { get; set; }
        public string Periodo { get; set; }
        public int TotalPendientes { get; set; }
        public int TotalPagados { get; set; }
        public int TotalAPagar { get; set; }
        public List<EstadoCuentaDetalleDto> Detalles { get; set; }
    }

    public class EstadoCuentaDetalleDto
    {
        public string Descripcion { get; set; }
        public int Monto { get; set; }
        public string FechaCargo { get; set; }
        public string FechaLimitePago { get; set; }
        public bool IsPagado { get; set; }
        public int IdMiembro { get; set; }
        public string ConceptoCodigo { get; set; }
        public string ConceptoNombre { get; set; }
    }

    public class EstadoCuentaProfile : Profile
    {
        public EstadoCuentaProfile()
        {
            CreateMap<Cargo, EstadoCuentaDto>(); 
        }
    }
}
