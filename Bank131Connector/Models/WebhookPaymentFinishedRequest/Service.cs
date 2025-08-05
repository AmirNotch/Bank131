using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class Service
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("amount_details")]
    public AmountDetails AmountDetails { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}