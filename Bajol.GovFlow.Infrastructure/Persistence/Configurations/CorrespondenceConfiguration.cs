using Bajol.GovFlow.Domain.Correspondences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations;

public sealed class CorrespondenceConfiguration : IEntityTypeConfiguration<Correspondence>
{
    public void Configure(EntityTypeBuilder<Correspondence> builder)
    {
        builder.ToTable("correspondences");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Subject).HasMaxLength(500).IsRequired();
        builder.Property(x => x.ReferenceNumber).HasMaxLength(64).IsRequired();
        builder.HasIndex(x => x.ReferenceNumber).IsUnique();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32);
        builder.Property(x => x.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(x => x.ModifiedBy).HasMaxLength(256);
    }
}
