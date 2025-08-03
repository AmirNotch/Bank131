using Bank131Connector.Models.db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank131Connector.Models.DbConfigurations;

public class Bank131PaymentConfiguration : IEntityTypeConfiguration<Bank131Payment>
{
    public void Configure(EntityTypeBuilder<Bank131Payment> entity)
    {
        entity.HasKey(e => e.PaymentId).HasName("bank131_payments_pkey");
        entity.ToTable("bank131_payments");

        entity.Property(e => e.PaymentId)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("payment_id")
            .HasColumnType("uuid")
            .IsRequired();

        entity.Property(e => e.SessionId)
            .HasColumnName("session_id")
            .HasColumnType("varchar(50)")
            .IsRequired();

        entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(20)")
            .IsRequired();

        entity.Property(e => e.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        entity.Property(e => e.FeeAmount)
            .HasColumnName("fee_amount")
            .HasColumnType("decimal(15,2)");

        entity.Property(e => e.CardLast4)
            .HasColumnName("card_last4")
            .HasColumnType("varchar(4)");

        entity.Property(e => e.CardBrand)
            .HasColumnName("card_brand")
            .HasColumnType("varchar(20)");

        entity.Property(e => e.Rrn)
            .HasColumnName("rrn")
            .HasColumnType("varchar(50)");

        entity.Property(e => e.AuthCode)
            .HasColumnName("auth_code")
            .HasColumnType("varchar(20)");

        entity.Property(e => e.RecipientName)
            .HasColumnName("recipient_name")
            .HasColumnType("varchar(200)");

        entity.Property(e => e.RecipientAccount)
            .HasColumnName("recipient_account")
            .HasColumnType("varchar(50)");

        entity.Property(e => e.RecipientBic)
            .HasColumnName("recipient_bic")
            .HasColumnType("varchar(20)");

        entity.Property(e => e.RecipientEmail)
            .HasColumnName("recipient_email")
            .HasColumnType("varchar(100)");

        entity.Property(e => e.FiscalReceiptLink)
            .HasColumnName("fiscal_receipt_link")
            .HasColumnType("text");

        entity.Property(e => e.FiscalTaxReference)
            .HasColumnName("fiscal_tax_reference")
            .HasColumnType("varchar(100)");

        entity.HasOne(e => e.Session)
            .WithOne(s => s.Payment)
            .HasForeignKey<Bank131Payment>(e => e.SessionId)
            .HasConstraintName("fk_bank131_payments_session");
        
        entity.HasIndex(e => e.SessionId)
            .IsUnique()
            .HasDatabaseName("idx_bank131_payments_session_unique");
    }
}
