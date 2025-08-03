using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class BankCard
{
    [JsonPropertyName("number")]
    public string Number { get; set; }
}