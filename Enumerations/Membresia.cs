namespace membresias.be.Enumerations
{
    public class Membresia : BaseEnumeration
    {
        public static readonly Membresia NoEncontrado = new("NE", "No encontrado", 0); 
        public static readonly Membresia Socio = new("SOCIO", "Socio", 1);
        public static readonly Membresia Invitado = new("INVITADO", "Invitado", 2); 

        public Membresia(string codigo, string nombre, short order) : base(codigo, nombre, order)
        {}

        public static Membresia GetByCode(string codigo) => MembresiaList.FirstOrDefault(m => m.Codigo.Equals(codigo)) ?? NoEncontrado; 

        public static List<Membresia> MembresiaList = new()
        {
            Socio,
            Invitado
        }; 
    }
}
