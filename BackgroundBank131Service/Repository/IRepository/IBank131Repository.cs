using BackgroundBank131Service.Models.db;

namespace BackgroundBank131Service.Repository.IRepository;

public interface IBank131Repository
{
    // public Task GettingSession(Bank131Session bank131Session, CancellationToken ct);
    public Task<List<Bank131Session>> GetSessionsForProcessingAsync(CancellationToken ct);
    public Task MarkSessionAsConfirmedAsync(string sessionId, CancellationToken ct);
}