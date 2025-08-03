using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class ParticipantDetails
{
    [JsonPropertyName("recipient")]
    public Recipient Recipient { get; set; }
}