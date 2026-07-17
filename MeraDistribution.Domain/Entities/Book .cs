using MiraDistribution.Domain.Common;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Domain.Exceptions;
using MiraDistribution.Domain.Common;
using MiraDistribution.Domain.Entities;
using MiraDistribution.Domain.Enums;
using MiraDistribution.Domain.Exceptions;

namespace MiraDistribution.Domain.Entities
{
    public class Book : BaseEntity
    {
        private const int ReceiptsPerBook = 50;

        public BookType Type { get; set; }
        public int SerialStart { get; private set; }
        public int SerialEnd { get; private set; }
        public BookStatus Status { get; set; } = BookStatus.NotAssigned;

        public int? DistributorId { get; set; }
        public Distributor? Distributor { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;

        public ICollection<BookAssignmentHistory> AssignmentHistories { get; set; } = new List<BookAssignmentHistory>();

        // EF Core محتاج constructor فاضي
        private Book() { }

        public Book(BookType type, int serialStart, string createdByUserId)
        {
            if (serialStart <= 0)
                throw new DomainException("SerialStart لازم يكون رقم موجب.");

            Type = type;
            SetSerialStart(serialStart);
            CreatedByUserId = createdByUserId;
            Status = BookStatus.NotAssigned;
        }

        public void SetSerialStart(int serialStart)
        {
            if (serialStart <= 0)
                throw new DomainException("SerialStart لازم يكون رقم موجب.");

            SerialStart = serialStart;
            SerialEnd = serialStart + ReceiptsPerBook - 1;
        }

        public bool ContainsReceiptNumber(int receiptNumber)
            => receiptNumber >= SerialStart && receiptNumber <= SerialEnd;

        public void AssignTo(int distributorId)
        {
            DistributorId = distributorId;
            Status = BookStatus.AssignedToDistributor;
        }

        public void Unassign()
        {
            DistributorId = null;
            Status = BookStatus.NotAssigned;
        }
    }
}