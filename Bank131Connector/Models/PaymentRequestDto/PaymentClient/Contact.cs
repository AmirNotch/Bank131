using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class Contact
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}