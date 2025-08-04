using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;
using Bank131Connector.Models.PaymentRequestDto.PaymentClient;

namespace Bank131Connector.Models.WebhookReadyToConfirmDto;
using System.Text.Json.Serialization;

public class PayoutListResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("customer")]
    public Customer Customer { get; set; }

    [JsonPropertyName("payout_details")]
    public PayoutDetailsResponse PayoutDetails { get; set; }

    [JsonPropertyName("amount_details")]
    public AmountDetails AmountDetails { get; set; }

    [JsonPropertyName("metadata")]
    public string Metadata { get; set; }
}