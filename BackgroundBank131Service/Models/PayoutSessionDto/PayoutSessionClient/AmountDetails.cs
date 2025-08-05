namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;
using System.Text.Json.Serialization;

public class AmountDetails
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}