using Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentCourseMapping : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        builder.ToTable("StudentCourses");
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.CreatedDate)
        .HasColumnType("timestamptz")
        .IsRequired();

        builder.Property(sc => sc.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(sc => sc.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
