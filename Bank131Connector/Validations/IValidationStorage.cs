using System.Net;

namespace Bank131Connector.Validations;

public interface IValidationStorage
{
    void AddError(ErrorCode errorCode, string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest);
    bool IsValid { get; }
    (ErrorCode, string) GetError();
    void Clear();
    HttpStatusCode HttpStatus { get; }
}
