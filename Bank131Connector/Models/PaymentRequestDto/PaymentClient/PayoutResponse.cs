using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class PayoutResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("session")]
    public SessionResponse Session { get; set; }
}