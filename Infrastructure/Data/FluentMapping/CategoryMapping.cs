using System;
using Domain.Entities;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(c => c.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));
        builder.OwnsOne(c => c.Description, d => d.Property(p => p.Text).HasMaxLength(255).IsRequired().HasColumnName("Description"));

        builder.HasOne(c => c.Picture)
            .WithOne(p => p.Category)
            .HasForeignKey<Picture>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Courses)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
