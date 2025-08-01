namespace Bank131Connector.Models.CurrencyRateDto;

public class CurrencyRate
{
    public int Id { get; set; }
    public string RateBegin { get; set; }
    public string CurrencyCode { get; set; }
    public int Type { get; set; }
    public decimal ValueBuy { get; set; }
    public decimal ValueSell { get; set; }
    public bool IsRateIncrease { get; set; }
}