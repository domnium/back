using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class ModuleMapping : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ToTable("Modules");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(m => m.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(m => m.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(m => m.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));
        builder.OwnsOne(m => m.Description, d => d.Property(p => p.Text).HasMaxLength(255).IsRequired().HasColumnName("Description"));

        builder.HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Lectures)
            .WithOne(l => l.Module)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
