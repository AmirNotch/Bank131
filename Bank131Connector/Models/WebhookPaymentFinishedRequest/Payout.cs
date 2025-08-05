using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class Payout
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("customer")]
    public Customer Customer { get; set; }

    [JsonPropertyName("payout_details")]
    public PayoutDetails PayoutDetails { get; set; }

    [JsonPropertyName("amount_details")]
    public AmountDetails AmountDetails { get; set; }

    [JsonPropertyName("amounts")]
    public PayoutAmounts Amounts { get; set; }

    [JsonPropertyName("fiscalization_details")]
    public FiscalizationDetails FiscalizationDetails { get; set; }

    [JsonPropertyName("transaction_info")]
    public TransactionInfo TransactionInfo { get; set; }
}