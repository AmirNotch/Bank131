using System.Net;
using BackgroundBank131Service.Validations;
using FluentValidation;

namespace BackgroundBank131Service.Utils;

public static class ValidationUtils
{
    public static string? GetValidationError<T>(T value, AbstractValidator<T> validator)
    {
        var validationResult = validator.Validate(value);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                return $"Property {error.PropertyName} failed validation. Error was: {error.ErrorMessage}";
            }
        }
        return null;
    }
    
    public static (ErrorCode code, string message) MapExternalError(string externalCode, string externalMessage)
    {
        return externalCode switch
        {
            "invalid_request" => (ErrorCode.InvalidRequest, $"Ошибка валидации: {externalMessage}"),
            "unauthorized" => (ErrorCode.Unauthorized, "Требуется авторизация"),
            "forbidden" => (ErrorCode.Forbidden, "Доступ запрещен"),
            "not_found" => (ErrorCode.InvalidData, "Ресурс не найден"),
            _ => (ErrorCode.ExternalServiceError, $"Ошибка внешнего сервиса: {externalMessage}")
        };
    }
    
    // Новый метод для добавления внешних ошибок
    public static void AddExternalApiError(
        IValidationStorage validationStorage, 
        string externalCode, 
        string externalMessage,
        HttpStatusCode statusCode)
    {
        var (mappedCode, mappedMessage) = MapExternalError(externalCode, externalMessage);
        validationStorage.AddError(mappedCode, mappedMessage, statusCode);
    }
    
    public static void AddApiError(IValidationStorage validationStorage, string code, string description, int statusCode)
    {
        validationStorage.AddError(
            ErrorCode.InvalidRequest,
            description,
            HttpStatusCode.Unauthorized
        );
    }

    public static void AddInvalidDataError(IValidationStorage validationStorage, Guid userId)
    {
        validationStorage.AddError(
            ErrorCode.InvalidData,
            $"InternalError {userId} does not exist",
            HttpStatusCode.NotFound
        );
    }

    public static void AddForbiddenError(IValidationStorage validationStorage)
    {
        validationStorage.AddError(
            ErrorCode.Forbidden,
            "Request data is empty",
            HttpStatusCode.BadRequest
        );
    }

    public static void AddInternalError(IValidationStorage validationStorage, Guid userId)
    {
        validationStorage.AddError(
            ErrorCode.InternalError,
            $"User with Id {userId} internal error",
            HttpStatusCode.NotFound
        );
    }
}