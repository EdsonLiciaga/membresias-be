using Microsoft.Identity.Client;

namespace membresias.be.Enumerations
{
    public class Concepto : BaseEnumeration
    {
        public static readonly Concepto NoEncontrado = new("NE", "No encontrado", 0);
        public static readonly Concepto AyudaMutua = new("AYUDA_MUTUA", "Ayuda mutua", 1);
        public static readonly Concepto CasaClub = new("CASA_CLUB", "Casa club", 2);
        public static readonly Concepto Reingreso = new("REINGRESO", "Reingreso", 3); 

        public Concepto(string codigo, string nombre, short order) : base(codigo, nombre, order)
        {}

        public static Concepto GetByCode(string codigo) => ConceptoList.FirstOrDefault(c => c.Codigo.Equals(codigo)) ?? NoEncontrado; 

        public static List<Concepto> ConceptoList = new()
        {
            AyudaMutua, 
            CasaClub, 
            Reingreso
        }; 
    }
}
