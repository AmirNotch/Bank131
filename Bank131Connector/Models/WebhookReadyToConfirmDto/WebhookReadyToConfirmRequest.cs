using System.Text.Json.Serialization;
using Bank131Connector.Validations.Attributes;

namespace Bank131Connector.Models.WebhookReadyToConfirmDto;

public class WebhookReadyToConfirmRequest
{
    [JsonPropertyName("type")]
    [ValidSession]
    public string Type { get; set; }

    [JsonPropertyName("session")]
    public SessionResponse Session { get; set; }
}