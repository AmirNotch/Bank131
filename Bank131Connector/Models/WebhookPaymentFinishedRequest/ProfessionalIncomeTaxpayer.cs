using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class ProfessionalIncomeTaxpayer
{
    [JsonPropertyName("services")]
    public List<Service> Services { get; set; }

    [JsonPropertyName("tax_reference")]
    public string TaxReference { get; set; }

    [JsonPropertyName("receipt")]
    public Receipt Receipt { get; set; }

    [JsonPropertyName("payer_type")]
    public string PayerType { get; set; }

    [JsonPropertyName("payer_name")]
    public string PayerName { get; set; }
}