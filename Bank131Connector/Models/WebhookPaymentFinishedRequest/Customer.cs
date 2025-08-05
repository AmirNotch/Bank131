using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class Customer
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; }
}