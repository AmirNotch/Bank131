namespace Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;

public class PaymentSessionClientRequest
{
    public AmountDetails AmountDetails { get; set; }
    
    public string Metadata { get; set; }
}