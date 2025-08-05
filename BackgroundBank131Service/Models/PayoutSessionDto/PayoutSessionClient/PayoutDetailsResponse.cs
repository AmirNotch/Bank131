using System.Text.Json.Serialization;

namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

public class PayoutDetailsResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("card")]
    public CardInfo Card { get; set; }
}