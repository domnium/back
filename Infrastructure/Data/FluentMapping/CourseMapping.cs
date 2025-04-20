using System;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class CourseMapping : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        // Table
        builder.ToTable("Courses");

        // Primary Key
        builder.HasKey(c => c.Id).HasName("PK_Courses");

        // Properties
        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired();

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

        builder.OwnsOne(r => r.NotionUrl, name =>
        {
            name.Property(n => n.Endereco)
                .HasColumnName("NotionUrl")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(r => r.GitHubUrl, name =>
        {
            name.Property(n => n.Endereco)
                .HasColumnName("GitHubUrl")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

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
            .HasColumnType("timestamp")
            .IsRequired(false);


        builder.Property(c => c.Price)
            .HasColumnName("Price")
            .HasColumnType("FLOAT")
            .IsRequired();

        builder.OwnsOne(r => r.AboutDescription, name =>
        {
            name.Property(n => n.Body)
                .HasColumnName("AboutDescription")
                .HasColumnType("varchar")
                .HasMaxLength(300)
                .IsRequired();
        });

        builder.Property(c => c.TotalHours)
            .HasColumnName("TotalHours")
            .HasColumnType("FLOAT")
            .IsRequired();

        builder.Property(c => c.Subscribes)
            .HasColumnName("Subscribes")
            .HasColumnType("BIGINT")
            .HasDefaultValue(0);

        builder.HasOne(i => i.IA)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.IAid)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Modules)
            .WithOne(m => m.Course)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany()
            .HasForeignKey(c => c.CategoryId)
            .HasConstraintName("FK_Course_Category")
            .OnDelete(DeleteBehavior.SetNull);  

        builder.HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .HasConstraintName("FK_Course_Teacher")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Image)
            .WithMany() 
            .HasForeignKey(c => c.PictureId) 
            .HasConstraintName("FK_Course_Picture")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Trailer)
            .WithOne()
            .HasConstraintName("FK_Course_Trailer_Video")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Parameters)
            .WithOne(p => p.Course)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
