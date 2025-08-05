using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookPaymentFinishedRequest;

public class FeeDetails
{
    [JsonPropertyName("merchant_fee")]
    public AmountDetails MerchantFee { get; set; }
}