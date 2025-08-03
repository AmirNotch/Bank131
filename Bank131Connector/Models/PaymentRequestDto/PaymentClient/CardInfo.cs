using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class CardInfo
{
    [JsonPropertyName("last4")]
    public string Last4 { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }
}