using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class PaymentDetailsResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("card")]
    public CardInfo Card { get; set; }
}