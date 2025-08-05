using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class RuBankAccountDetails
{
    [JsonPropertyName("bic")]
    public string Bic { get; set; }

    [JsonPropertyName("account")]
    public string Account { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("is_fast")]
    public bool IsFast { get; set; }
}