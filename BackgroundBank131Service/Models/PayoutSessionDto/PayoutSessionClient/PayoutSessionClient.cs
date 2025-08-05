using System.Text.Json.Serialization;

namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

public class PayoutSessionClient
{
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }
}