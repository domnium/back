using Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentCourseMap : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        builder.ToTable("StudentCourses");

        builder.HasKey(sc => sc.Id).HasName("PK_StudentCourses");

        builder.Property(sc => sc.Id)
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(sc => sc.EnrollmentDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(sc => sc.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(sc => sc.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(sc => sc.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        // FKs
        builder.HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sc => sc.Course)
            .WithMany()
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
