namespace membresias.be.Enumerations
{
    public class MiembroEstatus : BaseEnumeration
    {
        public static readonly MiembroEstatus NoEncontrado = new("NE", "No encontrado", 0);
        public static readonly MiembroEstatus Activo = new("ACTIVO", "Activo", 1);
        public static readonly MiembroEstatus Inactivo = new("INACTIVO", "Inactivo", 2);
        public static readonly MiembroEstatus Fallecido = new("FALLECIDO", "Fallecido", 3);

        public MiembroEstatus(string codigo, string nombre, short order) : base(codigo, nombre, order)
        {}

        public static MiembroEstatus GetByCode(string codigo) => MiembroEstatusList.FirstOrDefault(me => me.Codigo.Equals(codigo)) ?? NoEncontrado;

        public static List<MiembroEstatus> MiembroEstatusList = new()
        {
            Activo,
            Inactivo,
            Fallecido
        }; 
    }
}
