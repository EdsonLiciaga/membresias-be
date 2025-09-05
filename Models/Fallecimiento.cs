namespace membresias.be.Models
{
    public class Fallecimiento : Auditable
    {
        public int IdFallecimiento { get; set; }
        public DateTime FechaFallecimiento { get; set; }
        public int IdMiembro { get; set; }
        public Miembro Miembro { get; set; }
    }
}
