namespace BackgroundBank131Service.Models.db;

public class Bank131Session
{
    public string SessionId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Metadata { get; set; }
    public string? NextAction { get; set; }
    public string InitCardNumber { get; set; }
    public string InitCardType { get; set; }
    
    // Поле нужно для работы Фоновой задачи которая будет связываться с сервисом Банка 131 для подтверждения или оплаты
    public int BackgroundJobConfirmed { get; set; } = 0;
    
    public virtual Bank131Payment Payment { get; set; }
}