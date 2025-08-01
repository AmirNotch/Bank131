using Bank131Connector.Validations.Attributes;

namespace Bank131Connector.Models.CreatingPaymentSessionDto;

public class PaymentSessionRequest
{
    [ValidNotNegativeDecimal]
    public decimal Amount { get; set; }
    
    [ValidNotEmptyString]
    public string Currency { get; set; }
    
    [ValidNotEmptyString]
    public string FullName { get; set; }
}