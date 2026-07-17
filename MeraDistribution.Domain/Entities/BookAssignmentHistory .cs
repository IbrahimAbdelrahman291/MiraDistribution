using MiraDistribution.Domain.Common;

namespace MiraDistribution.Domain.Entities
{
    public class BookAssignmentHistory : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public int DistributorId { get; set; }
        public Distributor Distributor { get; set; } = null!;

        public string AssignedByUserId { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnassignedAt { get; set; }
    }
}
