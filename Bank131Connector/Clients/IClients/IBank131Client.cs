using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;

namespace Bank131Connector.Clients.IClients;

public interface IBank131Client {
    public Task<(bool isValid, PaymentSessionResponse? paymentSessionResponse)> SendPaymentSessionAsync(PaymentSessionRequest paymentSessionRequest);
}