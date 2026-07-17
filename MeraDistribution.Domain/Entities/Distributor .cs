using MiraDistribution.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraDistribution.Domain.Entities
{
    public class Distributor : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<BookAssignmentHistory> AssignmentHistories { get; set; } = new List<BookAssignmentHistory>();
    }
}
