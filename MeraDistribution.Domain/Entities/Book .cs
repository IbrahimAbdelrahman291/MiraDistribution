using MiraDistribution.Domain.Common;
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
        public DateTime? DeliveryDate { get; private set; }  
        public DateTime? ReceivedDate { get; private set; }    
        public string? Notes { get; set; }

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
            DeliveryDate = null;
            ReceivedDate = null;
        }

        public void Unassign()
        {
            DistributorId = null;
            Status = BookStatus.NotAssigned;
            DeliveryDate = null;
            ReceivedDate = null;
        }

        public void SetDeliveryDate(DateTime date) => DeliveryDate = date;
        public void SetReceivedDate(DateTime date) => ReceivedDate = date;
        public void SetNote(string? note) => Notes = note;
    }
}