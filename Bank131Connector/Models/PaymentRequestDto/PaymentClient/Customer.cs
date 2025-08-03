using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class Customer
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; }
}