using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Maps the <see cref="Role"/> aggregate to PostgreSQL with identity and audit columns.
/// </summary>
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid);

        builder.Property(x => x.Name)
            .HasMaxLength(IdentityColumnConstraints.Role.NameMaxLength)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(IdentityColumnConstraints.Role.DescriptionMaxLength);

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

        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName(IdentityColumnConstraints.Role.NameUniqueIndex)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
