using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Maps the <see cref="User"/> aggregate to PostgreSQL with identity and audit columns.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid);

        builder.Property(x => x.FullName)
            .HasMaxLength(IdentityColumnConstraints.User.FullNameMaxLength)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(IdentityColumnConstraints.User.EmailMaxLength)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(IdentityColumnConstraints.User.PhoneNumberMaxLength);

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(IdentityColumnConstraints.User.PasswordHashMaxLength)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlBoolean)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(x => x.LastLoginAt)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlTimestamptz);

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

        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName(IdentityColumnConstraints.User.EmailUniqueIndex)
            .HasFilter("\"IsDeleted\" = false");

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
