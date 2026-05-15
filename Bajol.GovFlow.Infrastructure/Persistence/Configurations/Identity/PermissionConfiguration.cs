using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Maps the <see cref="Permission"/> aggregate to PostgreSQL with identity and audit columns.
/// </summary>
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid);

        builder.Property(x => x.Name)
            .HasMaxLength(IdentityColumnConstraints.Permission.NameMaxLength)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasMaxLength(IdentityColumnConstraints.Permission.CodeMaxLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(IdentityColumnConstraints.Permission.DescriptionMaxLength);

        builder.Property(x => x.CreatedAt)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlTimestamptz)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .HasMaxLength(IdentityColumnConstraints.Audit.PrincipalMaxLength)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlTimestamptz);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(IdentityColumnConstraints.Audit.PrincipalMaxLength);

        builder.Property(x => x.IsDeleted)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlBoolean)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.DeletedAt)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlTimestamptz);

        builder.Property(x => x.DeletedBy)
            .HasMaxLength(IdentityColumnConstraints.Audit.PrincipalMaxLength);

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName(IdentityColumnConstraints.Permission.CodeUniqueIndex)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
