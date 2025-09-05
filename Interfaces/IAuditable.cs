namespace membresias.be.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
    }
}
