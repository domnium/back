
using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Domain.Entities.Core;

namespace Infrastructure.Data.FluentMapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasColumnType("uuid")
            
            .IsRequired();

        builder.Property(c => c.CreatedDate)
            .HasColumnName("CreatedDate")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.UpdatedDate)
            .HasColumnName("UpdatedDate")
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(c => c.DeletedDate)
            .HasColumnName("DeletedDate")
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.OwnsOne(u => u.FullName, fullName =>
        {
            fullName.Property(f => f.FirstName)
                .HasMaxLength(100)
                .HasColumnName("FirstName")
                .HasColumnType("varchar")
                .IsRequired();

            fullName.Property(f => f.LastName)
                .HasMaxLength(100)
                .HasColumnName("LastName")
                .HasColumnType("varchar")
                .IsRequired();
        });

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Address)
                .HasColumnName("Email")
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .IsRequired();
        });

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Road)
                .HasColumnName("Road")
                .HasMaxLength(100)
                .HasColumnType("varchar")
                .IsRequired(false);

            address.Property(a => a.Number)
                .HasColumnName("Number")
                .HasColumnType("bigint")
                .IsRequired(false);

            address.Property(a => a.NeighBordHood)
                .HasColumnName("NeighborHood")
                .HasColumnType("varchar")
                .IsRequired(false);

            address.Property(a => a.Complement)
                .HasColumnName("Complement")
                .HasMaxLength(100)
                .HasColumnType("varchar")
                .IsRequired(false);
        });

        builder.Property(u => u.TokenActivate)
            .HasColumnName("TokenActivate")
            .HasColumnType("varchar")
            .IsRequired(false);

        builder.Property(u => u.Active)
            .HasColumnName("Active")
            .HasColumnType("boolean")
            .IsRequired();

        builder.OwnsOne(u => u.Password, password =>
        {
            password.Property(p => p.Hash)
                .HasColumnName("Hash")
                .HasColumnType("varchar")
                .IsRequired(false);

            password.Property(p => p.Salt)
                .HasColumnName("Salt")
                .HasColumnType("varchar")
                .IsRequired(false);

            password.Ignore(p => p.Content);
        });

        builder.Property(u => u.StudentId)
            .HasColumnName("StudentId")
            .HasColumnType("uuid")
            .IsRequired(false);
        
        builder.HasOne(u => u.Student)
            .WithOne()
            .HasForeignKey<User>(s => s.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                role => role
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade),
                user => user
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
