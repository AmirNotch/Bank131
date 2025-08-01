using Bank131Connector.Models.db;
using Microsoft.EntityFrameworkCore;

namespace Bank131Connector.Models;

public partial class Bank131DbContext : DbContext
{
    public Bank131DbContext()
    {
    }

    public Bank131DbContext(DbContextOptions<Bank131DbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Bank131Session> Bank131Sessions { get; set; }
    public virtual DbSet<Bank131Payment> Bank131Payments { get; set; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string pgConnectionEnv = Environment.GetEnvironmentVariable("PG_CONNECTION") ??
                                     throw new ApplicationException("Environment variable PG_CONNECTION is not set!");
            optionsBuilder.UseNpgsql(pgConnectionEnv!);
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Bank131DbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}