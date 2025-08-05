using System.Text.Json.Serialization;

namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

public class PayoutSessionClientResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("session")]
    public SessionResponse Session { get; set; }
}