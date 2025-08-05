using System.Net;
using System.Text.Json;
using Bank131Connector.Clients.IClients;
using Bank131Connector.Models.ApiErrorDto;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;
using Bank131Connector.Models.PaymentRequestDto;
using Bank131Connector.Models.PaymentRequestDto.PaymentClient;
using Bank131Connector.Utils;
using Bank131Connector.Validations;
    

namespace Bank131Connector.Clients;

public class Bank131Client : IBank131Client
{
    private readonly HttpClient _httpClient;
    private readonly IValidationStorage _validationStorage;

    public Bank131Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<(bool isValid, PaymentSessionResponse? paymentSessionResponse)> SendPaymentSessionAsync(PaymentSessionClientRequest paymentSessionClientRequest, CancellationToken ct)
    {
        var session = await _httpClient.PostAsJsonAsync("/api/v2/session/create", paymentSessionClientRequest, cancellationToken: ct);
        
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

    public async Task<(bool isValid, PayoutResponse? paymentSessionResponse)> SendPayoutAsync(PaymentRequest paymentRequest)
    {
        var payout = await _httpClient.PostAsJsonAsync("/api/v2/session/start/payout", paymentRequest);
        
        if (!payout.IsSuccessStatusCode)
        {
            // Пытаемся прочитать тело ошибки как JSON
            try
            {
                var errorResponse = await payout.Content.ReadFromJsonAsync<ApiErrorResponse>();
            
                if (errorResponse?.Error != null)
                {
                    ValidationUtils.AddExternalApiError(
                        _validationStorage,
                        errorResponse.Error.Code,
                        errorResponse.Error.Description,
                        (HttpStatusCode)payout.StatusCode);
            
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
                await payout.Content.ReadAsStringAsync(),
                (int)payout.StatusCode);
    
            return (_validationStorage.IsValid, null);
        }
    
        var response = await payout.Content.ReadFromJsonAsync<PayoutResponse>();
        return (_validationStorage.IsValid, response);
    }
}