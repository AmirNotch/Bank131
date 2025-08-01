using System.Net;
using System.Text.Json;
using Bank131Connector.Clients.IClients;
using Bank131Connector.Models.ApiErrorDto;
using Bank131Connector.Models.CurrencyRateDto;
using Bank131Connector.Utils;
using Bank131Connector.Validations;

namespace Bank131Connector.Clients;

public class VaslClient : IVaslClient
{
    private readonly HttpClient _httpClient;
    private readonly IValidationStorage _validationStorage;
    
    public VaslClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool isValid, List<CurrencyRate>? currencyRates)> GetCurrencyRatesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/CurrencyRate");
        
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                
                    if (errorResponse?.Error != null)
                    {
                        ValidationUtils.AddExternalApiError(
                            _validationStorage,
                            errorResponse.Error.Code,
                            errorResponse.Error.Description,
                            (HttpStatusCode)response.StatusCode);
                    
                        return (false, null);
                    }
                }
                catch (JsonException)
                {
                    ValidationUtils.AddApiError(
                        _validationStorage, 
                        "invalid_response",
                        "Неверный формат ответа от сервера",
                        (int)HttpStatusCode.BadGateway);
                
                    return (false, null);
                }
            
                ValidationUtils.AddApiError(
                    _validationStorage,
                    "unknown_error",
                    await response.Content.ReadAsStringAsync(),
                    (int)response.StatusCode);

                return (false, null);
            }
        
            var rates = await response.Content.ReadFromJsonAsync<List<CurrencyRate>>();
            return (true, rates);
        }
        catch (Exception ex)
        {
            ValidationUtils.AddApiError(
                _validationStorage,
                "internal_error",
                $"Ошибка при получении курсов валют: {ex.Message}",
                (int)HttpStatusCode.InternalServerError);
        
            return (false, null);
        }
    }
}