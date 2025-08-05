using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class FiscalizationDetails
{
    [JsonPropertyName("professional_income_taxpayer")]
    public ProfessionalIncomeTaxpayer ProfessionalIncomeTaxpayer { get; set; }
}