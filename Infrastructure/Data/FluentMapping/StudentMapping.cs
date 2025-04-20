using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentMapping : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id).HasName("PK_Students");

        builder.Property(s => s.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        builder.Property(s => s.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(s => s.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(s => s.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(s => s.IsFreeStudent)
            .HasColumnName("IsFreeStudent")
            .HasColumnType("boolean")
            .IsRequired();

        builder.OwnsOne(s => s.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Picture)
            .WithMany()
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
