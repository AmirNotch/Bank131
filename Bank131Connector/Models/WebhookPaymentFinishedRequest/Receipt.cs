using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class Receipt
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }
}