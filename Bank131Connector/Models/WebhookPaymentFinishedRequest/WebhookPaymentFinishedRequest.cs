using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class WebhookPaymentFinishedRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("session")]
    public PaymentSession Session { get; set; }
}