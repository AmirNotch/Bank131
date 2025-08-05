using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class BankAccountDetails
{
    [JsonPropertyName("system_type")]
    public string SystemType { get; set; }

    [JsonPropertyName("ru")]
    public RuBankAccountDetails Ru { get; set; }
}