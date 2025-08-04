using Bank131Connector.Models;
using Bank131Connector.Models.db;
using Bank131Connector.Models.WebhookReadyToConfirmDto;
using Bank131Connector.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdatingSessionAndPayout(WebhookReadyToConfirmRequest webhookReadyToConfirmRequest, CancellationToken ct)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Обновление основной таблицы (Bank131Sessions)
            await _dbContext.Bank131Sessions
                .Where(s => s.SessionId == webhookReadyToConfirmRequest.Session.Id && s.Status == webhookReadyToConfirmRequest.Session.Status)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.NextAction, webhookReadyToConfirmRequest.Session.NextAction)
                    .SetProperty(s => s.UpdatedAt, webhookReadyToConfirmRequest.Session.UpdatedAt), 
                    cancellationToken: ct);

            // Обновление зависимой таблицы (Bank131Payments)
            await _dbContext.Bank131Payments
                .Where(p => p.PaymentId == webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.Id).FirstOrDefault())
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Status, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.Status).FirstOrDefault())
                    .SetProperty(p => p.CardLast4, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.PayoutDetails.Card.Last4).FirstOrDefault())
                    .SetProperty(p => p.CardBrand, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.PayoutDetails.Card.Brand).FirstOrDefault())
                    .SetProperty(p => p.RecipientEmail, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.PayoutDetails.Card.Brand).FirstOrDefault())
                    .SetProperty(p => p.CardBrand, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.PayoutDetails.Card.Brand).FirstOrDefault()), 
                    cancellationToken: ct);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}