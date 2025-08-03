using System.Text.Json.Serialization;

namespace Bank131Connector.Models.PaymentRequestDto;

public class Card
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("bank_card")]
    public BankCard BankCard { get; set; }
}