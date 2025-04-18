using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class VideoMapping : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        // Table
        builder.ToTable("Videos");

        // Primary Key
        builder.HasKey(v => v.Id).HasName("PK_Videos");

        // Properties (herdados de Archive + específicos)
        builder.Property(v => v.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        builder.Property(v => v.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(v => v.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(v => v.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(v => v.Ativo)
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(v => v.BucketName)
            .HasColumnName("BucketName")
            .HasColumnType("varchar")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(v => v.UrlExpired)
            .HasColumnName("UrlExpired")
            .HasColumnType("timestamp")
            .IsRequired(false);

        // Value Object: AwsKey
        builder.OwnsOne(v => v.AwsKey, aws =>
        {
            aws.Property(a => a.Body)
                .HasColumnName("AwsKey")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });


        builder.OwnsOne(v => v.TemporaryPath, tp =>
        {
            tp.Property(a => a.Body)
                .HasColumnName("TemporaryPath")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        // Value Object: UrlTemp
        builder.OwnsOne(v => v.UrlTemp, url =>
        {
            url.Property(u => u.Endereco)
                .HasColumnName("UrlTemp")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        builder.Property(p => p.ContentType)
            .HasColumnName("ContentType")
            .HasColumnType("varchar")
            .HasConversion<string>() 
            .IsRequired(false);
    }
}
