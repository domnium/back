using Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentLectureMapping : IEntityTypeConfiguration<StudentLecture>
{
    public void Configure(EntityTypeBuilder<StudentLecture> builder)
    {
        builder.ToTable("StudentLectures");
        builder.HasKey(sl => sl.Id);

        builder.Property(sl => sl.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(sl => sl.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(sl => sl.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.HasOne(sl => sl.Student)
            .WithMany(s => s.StudentLectures)
            .HasForeignKey(sl => sl.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sl => sl.Lecture)
            .WithMany(l => l.StudentLectures)
            .HasForeignKey(sl => sl.LectureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sl => sl.Course)
            .WithMany(c => c.StudentLectures)
            .HasForeignKey(sl => sl.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
