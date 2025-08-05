using BackgroundBank131Service.Models;
using BackgroundBank131Service.Models.db;
using BackgroundBank131Service.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BackgroundBank131Service.Repository;

public class Bank131Repository : IBank131Repository
{
    private readonly Bank131DbContext _dbContext;
    
    public Bank131Repository(Bank131DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Bank131Session>> GetSessionsForProcessingAsync(CancellationToken ct)
    {
        return await _dbContext.Bank131Sessions
            .Where(s => s.Status == "in_progress" 
                        && s.NextAction == null 
                        || s.NextAction == "ready_to_confirm"
                        && s.BackgroundJobConfirmed == 0)
            .OrderBy(s => s.CreatedAt)
            .Take(Constants.BatchSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task MarkSessionAsConfirmedAsync(string sessionId, CancellationToken ct)
    {
        await _dbContext.Bank131Sessions
            .Where(s => s.SessionId == sessionId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.BackgroundJobConfirmed, 1), ct);
    }
}