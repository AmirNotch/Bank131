using System.Net;
using System.Text.Json;
using Bank131Connector.Clients.IClients;
using Bank131Connector.Models.ApiErrorDto;
using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;
using Bank131Connector.Utils;
using Bank131Connector.Validations;
using IdGen;

namespace Bank131Connector.Clients;

public class Bank131Client : IBank131Client
{
    private readonly HttpClient _httpClient;
    private readonly IValidationStorage _validationStorage;

    public Bank131Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<(bool isValid, PaymentSessionResponse? paymentSessionResponse)> SendPaymentSessionAsync(PaymentSessionRequest paymentSessionRequest)
    {
        var generator = new IdGenerator(0);
        var id = generator.CreateId();
        var request = new PaymentSessionClientRequest
        {
            AmountDetails = new AmountDetails
            {
                Amount = ,
                Currency = "rub"
            },
            Metadata = $"order{id}"
        };
        
        var session = await _httpClient.PostAsJsonAsync("/api/v2/session/create", request);
        
        if (!session.IsSuccessStatusCode)
        {
            // Пытаемся прочитать тело ошибки как JSON
            try
            {
                var errorResponse = await session.Content.ReadFromJsonAsync<ApiErrorResponse>();
            
                if (errorResponse?.Error != null)
                {
                    ValidationUtils.AddExternalApiError(
                        _validationStorage,
                        errorResponse.Error.Code,
                        errorResponse.Error.Description,
                        (HttpStatusCode)session.StatusCode);
            
                    return (_validationStorage.IsValid, null);
                }
            }
            catch (JsonException)
            {
                ValidationUtils.AddApiError(
                    _validationStorage, 
                    "invalid_response",
                    "Неверный формат ответа от сервера",
                    (int)HttpStatusCode.BadGateway);
        
                return (_validationStorage.IsValid, null);
            }
            
            // Если не удалось распарсить ошибку
            ValidationUtils.AddApiError(
                _validationStorage,
                "unknown_error",
                await session.Content.ReadAsStringAsync(),
                (int)session.StatusCode);
    
            return (_validationStorage.IsValid, null);
        }
    
        var response = await session.Content.ReadFromJsonAsync<PaymentSessionResponse>();
        return (_validationStorage.IsValid, response);
    }
}