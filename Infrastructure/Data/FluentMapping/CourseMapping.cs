using Domain.Entities;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class CourseMapping : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.Id).HasName("PK_Courses");

        builder.Property(c => c.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);
        builder.Property(c => c.Price).IsRequired();
        builder.Property(c => c.TotalHours).IsRequired();
        builder.Property(c => c.Subscribes).HasDefaultValue(0);

        builder.OwnsOne(c => c.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));
        builder.OwnsOne(c => c.Description, d => d.Property(p => p.Text).HasMaxLength(255).IsRequired().HasColumnName("Description"));
        builder.OwnsOne(c => c.AboutDescription, a => a.Property(p => p.Body).HasMaxLength(300).IsRequired().HasColumnName("AboutDescription"));
        builder.OwnsOne(c => c.GitHubUrl, g => g.Property(p => p.Endereco).HasMaxLength(255).HasColumnName("GitHubUrl"));
        builder.OwnsOne(c => c.NotionUrl, n => n.Property(p => p.Endereco).HasMaxLength(255).IsRequired().HasColumnName("NotionUrl"));

        builder.HasMany(c => c.Modules)
            .WithOne(m => m.Course)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Category)
            .WithMany(ca => ca.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Picture)
            .WithOne(p => p.Course)
            .HasForeignKey<Picture>(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Trailer)
            .WithOne(v => v.Course)
            .HasForeignKey<Video>(v => v.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.IA)
            .WithMany(ia => ia.Courses)
            .HasForeignKey(c => c.IAid)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Parameters)
            .WithOne(p => p.Course)
            .HasForeignKey<Parameter>(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
