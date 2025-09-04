using DotnetTestTask.Core.TreeAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetTestTask.Infrastructure.Data.Config;

internal class NodeConfiguration
    : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("nodes");
        
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .HasConversion(id => id.Value, value => NodeId.Create(value))
            .ValueGeneratedOnAdd();

        builder.Property(n => n.ParentId)
            .HasConversion(
                id => id == null ? (long?)null : id.Value,
                value => value == null ? null : NodeId.Create(value.Value)
            );

        builder.Property(n => n.Id)
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(n => n.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(n => n.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(n => new { n.TreeId, n.ParentId, n.Name })
            .IsUnique();

        builder.Metadata
            .FindNavigation(nameof(Node.Children))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
