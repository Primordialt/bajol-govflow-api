using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations;

/// <summary>
/// Maps the <see cref="User"/> aggregate to PostgreSQL.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(32);
        builder.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.LastLoginAt).HasColumnType("timestamp with time zone");

        builder.Property(x => x.CreatedAt).HasColumnType("timestamp with time zone").IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(256).IsRequired();
        builder.Property(x => x.UpdatedAt).HasColumnType("timestamp with time zone");
        builder.Property(x => x.UpdatedBy).HasMaxLength(256);
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.DeletedAt).HasColumnType("timestamp with time zone");
        builder.Property(x => x.DeletedBy).HasMaxLength(256);

        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
