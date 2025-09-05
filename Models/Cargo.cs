namespace membresias.be.Models
{
    public class Cargo : Auditable
    {
        public int IdCargo { get; set; }
        public int Monto { get; set; }
        public DateTime FechaCargo { get; set; }
        public bool IsPagado { get; set; }
        public int IdMiembro { get; set; }
        public string ConceptoCodigo { get; set; }
        public Miembro Miembro { get; set; }
    }
}
