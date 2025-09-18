using membresias.be.Interfaces;

namespace membresias.be.Models
{
    public class Auditable : IAuditable
    {
        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6));
        public DateTimeOffset ModifiedDate { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToOffset(TimeSpan.FromHours(-6)); 
    }
}
