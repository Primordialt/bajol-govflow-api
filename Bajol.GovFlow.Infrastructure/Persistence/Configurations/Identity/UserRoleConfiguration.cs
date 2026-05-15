using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Maps the <see cref="UserRole"/> join entity (user-to-role many-to-many) to PostgreSQL.
/// </summary>
public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid);

        builder.Property(x => x.UserId)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.RoleId })
            .IsUnique()
            .HasDatabaseName(IdentityColumnConstraints.UserRole.UserRoleUniqueIndex);

        builder.HasIndex(x => x.RoleId)
            .HasDatabaseName(IdentityColumnConstraints.UserRole.RoleIdIndex);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
