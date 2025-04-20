using System;

namespace Infrastructure.Data.FluentMapping;

using System;
using Domain.Entities;
using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            
            .IsRequired();

        builder.Property(r => r.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(r => r.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(r => r.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(r => r.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(r => r.Slug)
            .HasColumnName("Slug")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                userRole => userRole
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade),
                userRole => userRole
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
