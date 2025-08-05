using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;
using Bank131Connector.Models.PaymentRequestDto;
using Bank131Connector.Models.PaymentRequestDto.PaymentClient;

namespace Bank131Connector.Clients.IClients;

public interface IBank131Client {
    public Task<(bool isValid, PaymentSessionResponse? paymentSessionResponse)> SendPaymentSessionAsync(PaymentSessionClientRequest paymentSessionClientRequest, CancellationToken ct);
    public Task<(bool isValid, PayoutResponse? paymentSessionResponse)> SendPayoutAsync(PaymentRequest paymentRequest);
}