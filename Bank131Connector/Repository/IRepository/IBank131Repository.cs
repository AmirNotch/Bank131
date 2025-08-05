using Bank131Connector.Models.db;
using Bank131Connector.Models.WebhookPaymentFinishedRequest;
using Bank131Connector.Models.WebhookReadyToConfirmDto;

namespace Bank131Connector.Repository.IRepository;

public interface IBank131Repository
{
    public Task CreateSession(Bank131Session bank131Session, CancellationToken ct);
    public Task CreatePayout(Bank131Payment bank131Payment, CancellationToken ct);
    public Task UpdatingSessionAndPayout(WebhookReadyToConfirmRequest webhookReadyToConfirmRequest, CancellationToken ct);
    public Task PaymentFinishedUpdatingSessionAndPayout(WebhookPaymentFinishedRequest webhookPaymentFinishedRequest, CancellationToken ct);
}