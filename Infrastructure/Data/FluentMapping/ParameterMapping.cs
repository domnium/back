using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class ParameterMapping : IEntityTypeConfiguration<Parameter>
{
    public void Configure(EntityTypeBuilder<Parameter> builder)
    {
        builder.ToTable("Parameters");

        builder.HasKey(p => p.Id).HasName("PK_Parameters");

        builder.Property(p => p.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        builder.Property(p => p.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(p => p.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(p => p.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.Property(p => p.FreeCourse)
            .HasColumnName("FreeCourse")
            .HasColumnType("boolean")
            .IsRequired(false);

        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(p => p.Description, desc =>
        {
            desc.Property(d => d.Text)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.HasOne(p => p.Course)
            .WithOne(c => c.Parameters)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
