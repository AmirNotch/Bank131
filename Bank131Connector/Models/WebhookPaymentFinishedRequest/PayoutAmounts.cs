using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class PayoutAmounts
{
    [JsonPropertyName("fee")]
    public FeeDetails Fee { get; set; }
}