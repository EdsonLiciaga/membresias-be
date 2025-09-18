namespace membresias.be.Models
{
    public class Fallecimiento : Auditable
    {
        public int IdFallecimiento { get; set; }
        public DateTimeOffset FechaFallecimiento { get; set; }
        public int IdMiembro { get; set; }
        public Miembro? Miembro { get; set; }
        public bool IsDeleted { get; set; }
    }
}
