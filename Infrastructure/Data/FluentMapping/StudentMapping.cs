using Domain.Entities;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentMapping : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(s => s.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(s => s.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);
        builder.Property(s => s.IsFreeStudent).IsRequired();

        builder.OwnsOne(s => s.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Picture)
            .WithOne(p => p.Student)
            .HasForeignKey<Picture>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.StudentCourses)
            .WithOne(sc => sc.Student)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.StudentLectures)
            .WithOne(sl => sl.Student)
            .HasForeignKey(sl => sl.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.Subscriptions)
            .WithOne(sub => sub.Student)
            .HasForeignKey(sub => sub.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

