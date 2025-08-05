using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class TransactionInfo
{
    [JsonPropertyName("rrn")]
    public string Rrn { get; set; }

    [JsonPropertyName("auth_code")]
    public string AuthCode { get; set; }
}