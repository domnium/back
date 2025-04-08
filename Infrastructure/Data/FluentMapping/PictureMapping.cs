using System;

namespace Infrastructure.Data.FluentMapping;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class PictureMap : IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.ToTable("Pictures");

        builder.HasKey(p => p.Id).HasName("PK_Pictures");

        builder.Property(p => p.Id)
            .HasColumnType("uuid")
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(p => p.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(p => p.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(p => p.Ativo)
            .HasColumnName("Ativo")
            .HasColumnType("boolean")
            .IsRequired();

        builder.Property(p => p.BucketName)
            .HasColumnName("BucketName")
            .HasColumnType("varchar")
            .HasMaxLength(255);

        builder.Property(p => p.UrlExpired)
            .HasColumnName("UrlExpired")
            .HasColumnType("timestamp");

        builder.OwnsOne(p => p.AwsKey, aws =>
        {
            aws.Property(a => a.Body)
                .HasColumnName("AwsKey")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(p => p.ContentType, ct =>
        {
            ct.Property(c => c.Type)
                .HasColumnName("ContentType")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired(false);
        });

        builder.OwnsOne(p => p.UrlTemp, url =>
        {
            url.Property(u => u.Endereco)
                .HasColumnName("UrlTemp")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });
    }
}
