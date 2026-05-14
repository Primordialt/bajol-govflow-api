using Bajol.GovFlow.Domain.Workflows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bajol.GovFlow.Infrastructure.Persistence.Configurations;

public sealed class WorkflowDefinitionConfiguration : IEntityTypeConfiguration<WorkflowDefinition>
{
    public void Configure(EntityTypeBuilder<WorkflowDefinition> builder)
    {
        builder.ToTable("workflow_definitions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Version).IsRequired();
        builder.Property(x => x.DefinitionDocument).HasColumnType("jsonb").IsRequired();
        builder.HasIndex(x => new { x.Key, x.Version }).IsUnique();
        builder.HasIndex(x => new { x.Key, x.IsPublished });
    }
}
