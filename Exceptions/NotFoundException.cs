namespace membresias.be.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, int id) : base($"No se encontró el id '{id}' en {entity}.") { } 
    }
}
