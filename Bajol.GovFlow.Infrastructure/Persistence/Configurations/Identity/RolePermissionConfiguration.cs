using Bajol.GovFlow.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations.Identity;

/// <summary>
/// Maps the <see cref="RolePermission"/> join entity (role-to-permission many-to-many) to PostgreSQL.
/// </summary>
public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid);

        builder.Property(x => x.RoleId)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid)
            .IsRequired();

        builder.Property(x => x.PermissionId)
            .HasColumnType(IdentityColumnConstraints.PostgreSqlUuid)
            .IsRequired();

        builder.HasIndex(x => new { x.RoleId, x.PermissionId })
            .IsUnique()
            .HasDatabaseName(IdentityColumnConstraints.RolePermission.RolePermissionUniqueIndex);

        builder.HasIndex(x => x.PermissionId)
            .HasDatabaseName(IdentityColumnConstraints.RolePermission.PermissionIdIndex);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
