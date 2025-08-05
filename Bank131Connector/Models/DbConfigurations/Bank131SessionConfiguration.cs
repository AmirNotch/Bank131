using Bank131Connector.Models.db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank131Connector.Models.DbConfigurations;

public class Bank131SessionConfiguration : IEntityTypeConfiguration<Bank131Session>
{
    public void Configure(EntityTypeBuilder<Bank131Session> entity)
    {
        entity.HasKey(e => e.SessionId).HasName("bank131_sessions_pkey");
        entity.ToTable("bank131_sessions"); 

        entity.Property(e => e.SessionId)
            .HasColumnName("session_id")
            .HasColumnType("varchar(50)")
            .IsRequired();

        entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasColumnType("varchar(50)")
            .IsRequired();

        entity.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        entity.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        entity.Property(e => e.Amount)
            .HasColumnName("amount")
            .HasColumnType("decimal(18,4)")
            .IsRequired();

        entity.Property(e => e.Currency)
            .HasColumnName("currency")
            .HasColumnType("varchar(3)")
            .IsRequired();

        entity.Property(e => e.Metadata)
            .HasColumnName("metadata")
            .HasColumnType("text")
            .IsRequired();

        entity.Property(e => e.NextAction)
            .HasColumnName("next_action")
            .HasColumnType("varchar(30)");

        entity.Property(e => e.InitCardNumber)
            .HasColumnName("init_card_number")
            .HasColumnType("varchar(16)")
            .IsRequired();

        entity.Property(e => e.InitCardType)
            .HasColumnName("init_card_type")
            .HasColumnType("varchar(20)")
            .IsRequired();
        
        entity.Property(e => e.BackgroundJobConfirmed)
            .HasColumnName("background_job_confirmed")
            .HasColumnType("int")
            .HasDefaultValue(0)
            .IsRequired();
    }
}