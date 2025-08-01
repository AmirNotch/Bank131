namespace Bank131Connector.Models.ActualCourse;

public class ActualCourseResponse
{
    public int Id { get; set; }
    public DateTime RateBegin { get; set; }
    public decimal CurrencyCode { get; set; }
    public int Type { get; set; }
    public decimal ValueBuy { get; set; }
    public decimal ValueSell { get; set; }
    public bool IsRateIncrease { get; set; }
}