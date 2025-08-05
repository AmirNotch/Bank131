using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class PayoutDetails
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("bank_account")]
    public BankAccountDetails BankAccount { get; set; }
}