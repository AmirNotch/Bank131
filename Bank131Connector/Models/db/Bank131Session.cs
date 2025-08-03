namespace Bank131Connector.Models.db;

public class Bank131Session
{
    public string SessionId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Metadata { get; set; }
    public string NextAction { get; set; }
    public string InitCardNumber { get; set; }
    public string InitCardType { get; set; }
    
    public virtual Bank131Payment Payment { get; set; }
}