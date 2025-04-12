using Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.FluentMapping;

public class StripeWebhookEventMapping : IEntityTypeConfiguration<StripeWebhookEvent>
{
    public void Configure(EntityTypeBuilder<StripeWebhookEvent> builder)
    {
        // Tabela
        builder.ToTable("StripeWebhookEvents");

        // Chave primária
        builder.HasKey(e => e.Id).HasName("PK_StripeWebhookEvents");

        builder.Property(e => e.Id)
            .HasColumnType("uuid")
            .IsRequired()
            ;

        // Datas
        builder.Property(e => e.CreatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(e => e.UpdatedDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(e => e.DeletedDate)
            .HasColumnType("timestamp")
            .IsRequired(false);

        builder.Property(e => e.ReceivedAt)
            .HasColumnName("ReceivedAt")
            .HasColumnType("timestamp")
            .IsRequired();

        // Propriedades simples
        builder.Property(e => e.EventId)
            .HasColumnName("EventId")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasColumnName("Type")
            .HasColumnType("varchar")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.PayloadJson)
            .HasColumnName("PayloadJson")
            .HasColumnType("jsonb")
            .IsRequired();
    }
}
