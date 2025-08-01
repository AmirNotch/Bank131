using System.Net;
using Bank131Connector.Validations;

namespace Bank131Connector.Extensions;

public static class ErrorCodeExtensions
{
    public static HttpStatusCode ToHttpStatus(this ErrorCode code)
    {
        return code switch
        {
            ErrorCode.InvalidData        => HttpStatusCode.BadRequest,        // 400 стандартная ошибка
            ErrorCode.Unauthorized       => HttpStatusCode.Unauthorized,      // 401 ошибка связана с авторизацией
            ErrorCode.Forbidden          => HttpStatusCode.Forbidden,         // 403
            ErrorCode.InternalError      => HttpStatusCode.InternalServerError, // 500
            _                            => HttpStatusCode.BadRequest         // default
        };
    }
}