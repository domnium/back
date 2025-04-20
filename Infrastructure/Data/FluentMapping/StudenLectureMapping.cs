using Domain.Entities.Relationships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StudentLectureMap : IEntityTypeConfiguration<StudentLecture>
{
    public void Configure(EntityTypeBuilder<StudentLecture> builder)
    {
        builder.ToTable("StudentLectures");

        builder.HasKey(sl => sl.Id).HasName("PK_StudentLectures");

        builder.Property(sl => sl.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        builder.Property(sl => sl.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(sl => sl.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(sl => sl.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(sl => sl.IsCompleted)
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(sl => sl.CompletionDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        // FKs
        builder.HasOne(sl => sl.Student)
            .WithMany(s => s.StudentLectures)
            .HasForeignKey(sl => sl.StudentId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sl => sl.Lecture)
            .WithMany(l => l.StudentLectures)
            .HasForeignKey(sl => sl.LectureId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sl => sl.Course)
            .WithMany()
            .HasForeignKey(sl => sl.CourseId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
