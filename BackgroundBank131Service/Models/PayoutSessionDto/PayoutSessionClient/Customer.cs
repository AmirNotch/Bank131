using System.Text.Json.Serialization;

namespace BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;

public class Customer
{
    [JsonPropertyName("reference")]
    public string Reference { get; set; }

    [JsonPropertyName("contacts")]
    public List<Contact> Contacts { get; set; }
}