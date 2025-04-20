using Domain.Entities;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class IAMapping : IEntityTypeConfiguration<IA>
{
    public void Configure(EntityTypeBuilder<IA> builder)
    {
        builder.ToTable("IAs");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(i => i.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(i => i.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(i => i.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));

        builder.HasOne(i => i.Picture)
            .WithOne(p => p.IA)
            .HasForeignKey<Picture>(p => p.IAId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Courses)
            .WithOne(c => c.IA)
            .HasForeignKey(c => c.IAid)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
