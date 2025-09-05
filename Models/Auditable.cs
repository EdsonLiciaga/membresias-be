using membresias.be.Interfaces;

namespace membresias.be.Models
{
    public class Auditable : IAuditable
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow; 
    }
}
