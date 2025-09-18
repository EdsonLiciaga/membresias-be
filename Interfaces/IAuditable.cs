namespace membresias.be.Interfaces
{
    public interface IAuditable
    {
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset ModifiedDate { get; set; }
    }
}
