using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookReadyToConfirmDto;

public class PayoutDetailsResponse
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("card")]
    public CardInfoResponse Card { get; set; }
}