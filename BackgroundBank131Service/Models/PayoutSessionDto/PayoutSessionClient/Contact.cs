using System.Text.Json.Serialization;

namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

public class Contact
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}