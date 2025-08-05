using BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

namespace BackgroundBank131Service.Clients.IClients;

public interface IBank131Client
{
    public Task<(bool isValid, PayoutSessionClientResponse? paymentSessionResponse)> SendPaymentSessionAsync(PayoutSessionClient payoutSessionClient,
        int status, CancellationToken ct);
}