using Bank131Connector.Models.db;

namespace Bank131Connector.Repository.IRepository;

public interface IBank131Repository
{
    public Task CreateSession(Bank131Session bank131Session, CancellationToken ct);
    public Task CreatePayout(Bank131Payment bank131Payment, CancellationToken ct);
}