using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class LectureMapping : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        // Table
        builder.ToTable("Lectures");

        // Primary Key
        builder.HasKey(l => l.Id).HasName("PK_Lectures");

        // Properties
        builder.Property(l => l.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(l => l.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(l => l.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(l => l.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(l => l.Tempo)
            .HasColumnName("Tempo")
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(l => l.Views)
            .HasColumnName("Views")
            .HasColumnType("bigint")
            .HasDefaultValue(0);

        // Value Object: Name
        builder.OwnsOne(l => l.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Value Object: GithubUrl
        builder.OwnsOne(l => l.GithubUrl, url =>
        {
            url.Property(u => u.Endereco)
                .HasColumnName("GithubUrl")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        // Relationships
        builder.HasOne(l => l.Module)
            .WithMany(m => m.Lectures)
            .HasForeignKey("ModuleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(l => l.Video)
            .WithOne()
            .HasForeignKey("VideoId")
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(l => l.StudentLectures)
            .WithOne(sl => sl.Lecture)
            .HasForeignKey(sl => sl.LectureId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
