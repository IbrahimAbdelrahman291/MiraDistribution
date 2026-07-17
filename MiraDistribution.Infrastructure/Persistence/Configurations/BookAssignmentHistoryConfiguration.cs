using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Infrastructure.Persistence.Configurations
{
    public class BookAssignmentHistoryConfiguration : IEntityTypeConfiguration<BookAssignmentHistory>
    {
        public void Configure(EntityTypeBuilder<BookAssignmentHistory> builder)
        {
            builder.ToTable("BookAssignmentHistories");

            builder.Property(h => h.AssignedByUserId).IsRequired();

            builder.HasOne(h => h.Book)
                .WithMany(b => b.AssignmentHistories)
                .HasForeignKey(h => h.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(h => h.Distributor)
                .WithMany(d => d.AssignmentHistories)
                .HasForeignKey(h => h.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
