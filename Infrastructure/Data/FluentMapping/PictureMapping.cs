
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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
            ;

        builder.Property(p => p.CreatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(p => p.UpdatedDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(p => p.DeletedDate)
            .HasColumnType("timestamptz")
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
            .HasColumnType("timestamptz");

        builder.OwnsOne(p => p.AwsKey, aws =>
        {
            aws.Property(a => a.Body)
                .HasColumnName("AwsKey")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

       builder.Property(p => p.ContentType)
        .HasColumnName("ContentType")
        .HasColumnType("varchar")
        .HasConversion<string>() 
        .IsRequired(false);

        builder.OwnsOne(v => v.TemporaryPath, tp =>
        {
            tp.Property(a => a.Body)
                .HasColumnName("TemporaryPath")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(p => p.UrlTemp, url =>
        {
            url.Property(u => u.Endereco)
                .HasColumnName("UrlTemp")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        builder.HasOne(p => p.Teacher)
            .WithOne(t => t.Picture)
            .HasForeignKey<Picture>(p => p.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Student)
            .WithOne(s => s.Picture)
            .HasForeignKey<Picture>(p => p.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Category)
            .WithOne(c => c.Picture)
            .HasForeignKey<Picture>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Course)
            .WithOne(c => c.Picture)
            .HasForeignKey<Picture>(p => p.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.IA)
            .WithOne(i => i.Picture)
            .HasForeignKey<Picture>(p => p.IAId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
