using membresias.be.Enumerations;

namespace membresias.be.Models
{
    public class Miembro : Auditable
    {
        public int IdMiembro { get; set; }
        public string? Clave { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Curp { get; set; }
        public DateTimeOffset FechaNacimiento { get; set; }
        public int Edad { get; set; }
        public DateTimeOffset FechaIngreso { get; set; }
        public DateTimeOffset? FechaReingreso { get; set; }
        public string MembresiaCodigo { get; set; }
        public string? MiembroEstatusCodigo { get; set; } = MiembroEstatus.Activo.Codigo; 
        public bool IsDeleted { get; set; }
    }
}
