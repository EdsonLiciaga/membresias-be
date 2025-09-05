using Microsoft.Identity.Client;

namespace membresias.be.Models
{
    public class Tarifa : Auditable
    {
        public int IdTarifa { get; set; }
        public string Nombre { get; set; }
        public int Monto { get; set; }
        public string MembresiaCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
    }
}
