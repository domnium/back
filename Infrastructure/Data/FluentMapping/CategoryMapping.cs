using System;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;
public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        //Table
        builder.ToTable("Categories");

        //Primary Key
        builder.HasKey(c => c.Id).HasName("PK_Categories");

        //Properties
        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(c => c.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(c => c.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(c => c.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamp").IsRequired(false);

        builder.OwnsOne(r => r.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

             builder.OwnsOne(r => r.Description, name =>
        {
            name.Property(n => n.Text)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.HasOne(c => c.Image)
            .WithMany()
            .HasForeignKey(c => c.PictureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

