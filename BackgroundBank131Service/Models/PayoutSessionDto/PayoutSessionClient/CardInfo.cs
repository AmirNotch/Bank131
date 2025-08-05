namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;
using System.Text.Json.Serialization;

public class CardInfo
{
    [JsonPropertyName("last4")]
    public string Last4 { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }
}