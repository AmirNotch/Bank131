using System.Net;
using System.Text.Json;
using BackgroundBank131Service.Clients.IClients;
using BackgroundBank131Service.Models.ApiErrorDto;
using BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;
using BackgroundBank131Service.Utils;
using BackgroundBank131Service.Validations;

namespace BackgroundBank131Service.Clients;

public class Bank131Client : IBank131Client
{
    private readonly HttpClient _httpClient;
    private readonly IValidationStorage _validationStorage;

    public Bank131Client(HttpClient httpClient, IValidationStorage validationStorage)
    {
        _httpClient = httpClient;
        _validationStorage = validationStorage;
    }

    public async Task<(bool isValid, PayoutSessionClientResponse? paymentSessionResponse)> SendPaymentSessionAsync(PayoutSessionClient payoutSessionClient,
        int status, CancellationToken ct)
    {

        var endpoint = status switch
        {
            2 => "/api/v2/session/confirm", // 2 означает можно подтвердить оплату
            3 => "/api/v2/session/cancel", // 3 означает оплату надо отменить
            _ => ""
        };
        
        var session = await _httpClient.PostAsJsonAsync(endpoint, payoutSessionClient, cancellationToken: ct);
        
        if (!session.IsSuccessStatusCode)
        {
            // Пытаемся прочитать тело ошибки как JSON
            try
            {
                var errorResponse = await session.Content.ReadFromJsonAsync<ApiErrorResponse>(ct);
            
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
                await session.Content.ReadAsStringAsync(ct),
                (int)session.StatusCode);
    
            return (_validationStorage.IsValid, null);
        }
    
        var response = await session.Content.ReadFromJsonAsync<PayoutSessionClientResponse>(ct);
        return (_validationStorage.IsValid, response);
    }
}