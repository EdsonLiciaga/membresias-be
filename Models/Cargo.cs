namespace membresias.be.Models
{
    public class Cargo : Auditable
    {
        public int IdCargo { get; set; }
        public string Descripcion { get; set; }
        public int Monto { get; set; }
        public DateTimeOffset FechaCargo { get; set; }
        public bool IsPagado { get; set; } = false; 
        public int IdMiembro { get; set; }
        public string ConceptoCodigo { get; set; }
        public Miembro Miembro { get; set; }
        public bool IsDeleted { get; set; }
    }
}
