namespace membresias.be.Models
{
    public class Miembro : Auditable
    {
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Curp { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaReingreso { get; set; }
        public string MembresiaCodigo { get; set; } 
        public string MiembroEstatusCodigo { get; set; }
    }
}
