namespace Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;

public class PaymentSessionResponse
{
    public decimal Amount { get; set; }
    public SessionDto Session { get; set; }
}