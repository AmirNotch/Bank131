using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto.PaymentClient;

public class SessionResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("payout_list")]
    public List<PayoutList> PayoutLists { get; set; }
}