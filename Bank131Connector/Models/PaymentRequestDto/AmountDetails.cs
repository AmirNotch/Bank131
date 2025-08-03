using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class AmountDetails
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}