namespace membresias.be.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string entity, string message) : base($"{entity}. {message}") {}
    }
}
