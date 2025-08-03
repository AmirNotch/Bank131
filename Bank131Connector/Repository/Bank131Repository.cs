using Bank131Connector.Models;
using Bank131Connector.Models.db;
using Bank131Connector.Repository.IRepository;

namespace Bank131Connector.Repository;

public class Bank131Repository : IBank131Repository
{
    private readonly Bank131DbContext _dbContext;
    
    public async Task CreateSession(Bank131Session bank131Session, CancellationToken ct)
    {
        _dbContext.Add(bank131Session);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task CreatePayout(Bank131Payment bank131Payment, CancellationToken ct)
    {
        _dbContext.Add(bank131Payment);
        await _dbContext.SaveChangesAsync(ct);
    }
}