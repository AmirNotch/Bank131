using System.Net;

namespace BackgroundBank131Service.Validations;

public class ValidationStorage : IValidationStorage
{
    private List<(ErrorCode, string)> _storage;

    public HttpStatusCode HttpStatus { get; private set; } = HttpStatusCode.OK;
    
    public ValidationStorage()
    {
        _storage = [];
    }
    
    public void AddError(ErrorCode errorCode, string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        _storage.Add((errorCode, errorMessage));
        HttpStatus = statusCode;
    }

    public bool IsValid => _storage.Count == 0;
    

    public void Clear()
    {
        _storage.Clear();
        HttpStatus = HttpStatusCode.OK;
    }

    public (ErrorCode, string) GetError()
    {
        return _storage[0];
    }

    public override string ToString()
    {
        string payload = string.Join(",", _storage.Select(pair => $"{{\"{pair.Item1}\":\"{pair.Item2}\"}}").ToList());
        return $"Status: {(int)HttpStatus} {HttpStatus}, Errors: [{payload}]";
    }
}