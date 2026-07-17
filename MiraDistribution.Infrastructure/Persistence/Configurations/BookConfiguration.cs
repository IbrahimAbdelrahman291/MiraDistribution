using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.Property(b => b.Type).HasConversion<int>().IsRequired();
            builder.Property(b => b.Status).HasConversion<int>().IsRequired();
            builder.Property(b => b.CreatedByUserId).IsRequired();

            builder.HasIndex(b => new { b.SerialStart, b.SerialEnd });

            builder.HasOne(b => b.Distributor)
                .WithMany(d => d.Books)
                .HasForeignKey(b => b.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
