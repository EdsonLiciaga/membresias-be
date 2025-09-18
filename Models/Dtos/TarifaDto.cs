using AutoMapper;

namespace membresias.be.Models.Dtos
{
    public class TarifaDto
    {
        public int IdTarifa { get; set; }
        public string Nombre { get; set; }
        public int Monto { get; set; }
        public string MembresiaCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
    }

    public class TarifaProfile : Profile
    {
        public TarifaProfile()
        {
            CreateMap<Tarifa, TarifaDto>(); 
        }
    }
}
