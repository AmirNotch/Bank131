using Bank131Connector.Models.CurrencyRateDto;

namespace Bank131Connector.Clients.IClients;

public interface IVaslClient
{
    public Task<(bool isValid, List<CurrencyRate>? currencyRates)> GetCurrencyRatesAsync();
}