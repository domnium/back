using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class VideoMapping : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.ToTable("Videos");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Ativo).IsRequired();
        builder.Property(v => v.BucketName).HasMaxLength(255);
        builder.Property(v => v.UrlExpired);
        builder.Property(v => v.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();
        builder.Property(v => v.ContentType)
            .HasColumnName("ContentType")
            .HasColumnType("varchar")
            .HasConversion<string>() 
            .IsRequired(false);

        builder.Property(v => v.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(v => v.DeletedDate)
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(v => v.AwsKey, aws =>
        {
            aws.Property(a => a.Body)
               .HasColumnName("AwsKey")
               .HasMaxLength(255)
               .IsRequired();
        });

        builder.OwnsOne(v => v.TemporaryPath, tp =>
        {
            tp.Property(a => a.Body)
              .HasColumnName("TemporaryPath")
              .HasMaxLength(255)
              .IsRequired();
        });

        builder.OwnsOne(v => v.UrlTemp, url =>
        {
            url.Property(u => u.Endereco)
                .HasColumnName("UrlTemp")
                .HasMaxLength(255);
        });

        builder.HasOne(v => v.Lecture)
        .WithOne(l => l.Video)
        .HasForeignKey<Video>(v => v.LectureId)
        .IsRequired(false)
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Course)
            .WithOne(c => c.Trailer)
            .HasForeignKey<Video>(v => v.CourseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
