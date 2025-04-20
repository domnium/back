using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class LectureMapping : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lectures");
        builder.HasKey(l => l.Id);

       builder.Property(l => l.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(l => l.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(l => l.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);
        builder.Property(l => l.Tempo).HasMaxLength(20).IsRequired();
        builder.Property(l => l.Views).HasDefaultValue(0);

        builder.OwnsOne(l => l.Name, n => n.Property(p => p.Name).HasMaxLength(100).IsRequired().HasColumnName("Name"));
        builder.OwnsOne(l => l.GithubUrl, g => g.Property(p => p.Endereco).HasMaxLength(255).HasColumnName("GithubUrl"));

        builder.HasOne(l => l.Module)
            .WithMany(m => m.Lectures)
            .HasForeignKey(l => l.ModuleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Video)
            .WithOne(v => v.Lecture)
            .HasForeignKey<Video>(v => v.LectureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(l => l.StudentLectures)
            .WithOne(sl => sl.Lecture)
            .HasForeignKey(sl => sl.LectureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
