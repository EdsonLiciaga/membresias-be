namespace membresias.be.Enumerations
{
    public abstract class BaseEnumeration
    {
        protected BaseEnumeration(string codigo, string nombre, short order)
        {
            Codigo = codigo;
            Nombre = nombre;
            Order = order; 
        }

        public string Codigo { get; }
        public string Nombre { get; }
        public short Order { get; }
    }
}
