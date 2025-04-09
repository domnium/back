using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class ModuleMapping : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        // Table
        builder.ToTable("Modules");

        // Primary Key
        builder.HasKey(m => m.Id).HasName("PK_Modules");

        // Properties
        builder.Property(m => m.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(m => m.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(m => m.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(m => m.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamp")
            .IsRequired(false);

        // Value Object: Name
        builder.OwnsOne(m => m.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Value Object: Description
        builder.OwnsOne(m => m.Description, desc =>
        {
            desc.Property(d => d.Text)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        // Relationships
        builder.HasOne(m => m.Course)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m => m.Lectures)
            .WithOne(l => l.Module)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
