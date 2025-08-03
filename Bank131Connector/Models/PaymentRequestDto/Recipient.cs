using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class Recipient
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
}