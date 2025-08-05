using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class Contact
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}