namespace Bank131Connector.Models.db;

public class Bank131Payment
{
    public Guid PaymentId { get; set; }
    public string SessionId { get; set; }
    public string Status { get; set; }
    public decimal Amount { get; set; }
    public decimal? FeeAmount { get; set; }
    public string CardLast4 { get; set; }
    public string? CardBrand { get; set; }
    public string? Rrn { get; set; }
    public string? AuthCode { get; set; }
    public string RecipientName { get; set; }
    public string? RecipientAccount { get; set; }
    public string? RecipientBic { get; set; }
    public string? RecipientEmail { get; set; }
    public string? FiscalReceiptLink { get; set; }
    public string? FiscalTaxReference { get; set; }
    
    public Bank131Session Session { get; set; }
}