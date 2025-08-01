using Bank131Connector.Clients.IClients;
using Bank131Connector.Models.ActualCourse;
using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Repository.IRepository;
using Bank131Connector.Validations;

namespace Bank131Connector.Services;

public class Bank131Service
{
    private readonly IBank131Repository _bank131Repository;
    private readonly IBank131Client _bank131Client;
    private readonly IVaslClient _vaslClient;
    private readonly IValidationStorage _validationStorage;
    
    public Bank131Service(IBank131Repository bank131Repository, IValidationStorage validationStorage)
    {
        _bank131Repository = bank131Repository;
        _validationStorage = validationStorage;
    }

    #region Action

    public async Task<bool> CreatingPaymentSession(PaymentSessionRequest paymentSessionRequest, CancellationToken ct)
    {
        // var (isValidRates, rates) = await _vaslClient.GetCurrencyRatesAsync();
        // if (!isValidRates)
        // {
        //     return false;
        // }
        
        var (isValidSession, response) = await _bank131Client.SendPaymentSessionAsync(paymentSessionRequest);
        if (!isValidSession)
        {
            return false;
        }

    }

    #endregion
    
    #region Validations
    
    
    
    #endregion
}