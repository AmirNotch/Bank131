using Bank131Connector.Models;
using Bank131Connector.Models.db;
using Bank131Connector.Models.WebhookPaymentFinishedRequest;
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
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

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
                    .SetProperty(p => p.RecipientEmail, webhookReadyToConfirmRequest.Session.PayoutLists.Select(p => p.PayoutDetails.Card.Brand).FirstOrDefault()), 
                    cancellationToken: ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
    
    public async Task PaymentFinishedUpdatingSessionAndPayout(WebhookPaymentFinishedRequest webhookPaymentFinishedRequest, CancellationToken ct)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            // Обновление основной таблицы (Bank131Sessions)
            await _dbContext.Bank131Sessions
                .Where(s => s.SessionId == webhookPaymentFinishedRequest.Session.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, webhookPaymentFinishedRequest.Session.Status)
                    .SetProperty(s => s.NextAction, "finished")
                    .SetProperty(s => s.UpdatedAt, webhookPaymentFinishedRequest.Session.UpdatedAt),
                    cancellationToken: ct);

            // Обновление зависимой таблицы (Bank131Payments)
            await _dbContext.Bank131Payments
                .Where(p => p.PaymentId == webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.Id).FirstOrDefault())
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(p => p.Status, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.Status).FirstOrDefault())
                    .SetProperty(p => p.RecipientBic, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.PayoutDetails.BankAccount.Ru.Bic).FirstOrDefault())
                    .SetProperty(p => p.FeeAmount, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.Amounts.Fee.MerchantFee.Amount).FirstOrDefault())
                    .SetProperty(p => p.Rrn, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.TransactionInfo.Rrn).FirstOrDefault())
                    .SetProperty(p => p.AuthCode, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.TransactionInfo.AuthCode).FirstOrDefault()) 
                    .SetProperty(p => p.FiscalTaxReference, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.FiscalizationDetails.ProfessionalIncomeTaxpayer.TaxReference).FirstOrDefault()) 
                    .SetProperty(p => p.FiscalReceiptLink, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.FiscalizationDetails.ProfessionalIncomeTaxpayer.Receipt.Link).FirstOrDefault()) 
                    .SetProperty(p => p.RecipientAccount, webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.PayoutDetails.BankAccount.Ru.Account).FirstOrDefault()), 
                    cancellationToken: ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}