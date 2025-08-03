using System.Text.Json.Serialization;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;

namespace Bank131Connector.Models.PaymentRequestDto;

public class PaymentRequest
{
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }

    [JsonPropertyName("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    [JsonPropertyName("participant_details")]
    public ParticipantDetails ParticipantDetails { get; set; }

    [JsonPropertyName("amount_details")]
    public AmountDetails AmountDetails { get; set; }

    [JsonPropertyName("metadata")]
    public string Metadata { get; set; }
}