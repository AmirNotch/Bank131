using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class PaymentMethod
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("card")]
    public Card Card { get; set; }
}