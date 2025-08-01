using System.Security.Cryptography;
using System.Text;
using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Services;
using Bank131Connector.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Bank131Connector.Controllers;

public class Bank131Controller : BaseController
{
    private readonly Bank131Service _back131service;
    
    protected Bank131Controller(IValidationStorage validationStorage, Bank131Service back131service) : base(validationStorage)
    {
        _back131service = back131service;
    }
    
    [HttpGet("getActualCourseInfo")]
    public async Task<IActionResult> GetActualCourseInfo(CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _back131service.GetActualCourseInfo(token), ct);
    }
    
    [HttpGet("CreatingPaymentSession")]
    public async Task<IActionResult> CreatingPaymentSession([FromBody] PaymentSessionRequest paymentSessionRequest,
        [FromHeader(Name = "X-Auth-Hash")] string clientHash, CancellationToken ct)
    {
        return await HandleRequestAsync(async token => await _back131service.CreatingPaymentSession(paymentSessionRequest, token), ct);
    }
}