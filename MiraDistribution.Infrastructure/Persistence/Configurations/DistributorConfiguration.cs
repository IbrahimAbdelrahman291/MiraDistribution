using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiraDistribution.Domain.Entities;

namespace MiraDistribution.Infrastructure.Persistence.Configurations
{
    public class DistributorConfiguration : IEntityTypeConfiguration<Distributor>
    {
        public void Configure(EntityTypeBuilder<Distributor> builder)
        {
            builder.ToTable("Distributors");

            builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
            builder.Property(d => d.Phone).IsRequired().HasMaxLength(20);
            builder.Property(d => d.UserId).IsRequired();

            builder.HasIndex(d => d.UserId).IsUnique();
        }
    }
}
