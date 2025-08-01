using System.Text.Json.Serialization;

namespace Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;

public class AmountDetails
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}