using System.Text.Json.Serialization;

namespace Bank131Connector.Models.WebhookReadyToConfirmDto;

public class CardInfoResponse
{
    [JsonPropertyName("last4")]
    public string Last4 { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }
    
    [JsonPropertyName("card_id")]
    public string CardId { get; set; }
}