using Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class TeacherMapping : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Table
        builder.ToTable("Teachers");

        // Primary Key
        builder.HasKey(t => t.Id).HasName("PK_Teachers");

        // Properties
        builder.Property(t => t.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        builder.Property(t => t.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(t => t.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(t => t.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(t => t.Phone)
            .HasColumnType("varchar")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(t => t.Cep)
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        // ValueObjects

        builder.OwnsOne(t => t.Name, name =>
        {
            name.Property(n => n.Name)
                .HasColumnName("Name")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Email, email =>
        {
            email.Property(e => e.Address)
                .HasColumnName("Email")
                .HasColumnType("varchar")
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Cpf, cpf =>
        {
            cpf.Property(c => c.Numero)
                .HasColumnName("Cpf")
                .HasColumnType("varchar")
                .HasMaxLength(14)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Endereco, e =>
        {
            e.Property(e => e.Body)
                .HasColumnName("Endereco")
                .HasColumnType("varchar")
                .HasMaxLength(300)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Description, d =>
        {
            d.Property(d => d.Text)
                .HasColumnName("Description")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired();
        });

        builder.OwnsOne(t => t.Tiktok, tk =>
        {
            tk.Property(t => t.Endereco)
                .HasColumnName("Tiktok")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        builder.OwnsOne(t => t.Instagram, insta =>
        {
            insta.Property(i => i.Endereco)
                .HasColumnName("Instagram")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        builder.OwnsOne(t => t.GitHub, gh =>
        {
            gh.Property(g => g.Endereco)
                .HasColumnName("GitHub")
                .HasColumnType("varchar")
                .HasMaxLength(255)
                .IsRequired(false);
        });

        // Relacionamento com Picture
        builder.HasOne(t => t.Picture)
            .WithMany()
            .HasForeignKey(t => t.PictureId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacionamento com Course
        builder.HasMany(t => t.Courses)
            .WithOne(c => c.Teacher)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
