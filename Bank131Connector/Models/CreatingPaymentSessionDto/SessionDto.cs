namespace Bank131Connector.Models.CreatingPaymentSessionDto;

public class SessionDto
{
    public string Id { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}