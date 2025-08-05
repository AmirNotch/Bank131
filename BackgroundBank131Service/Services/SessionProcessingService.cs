using BackgroundBank131Service.Clients.IClients;
using BackgroundBank131Service.Models;
using BackgroundBank131Service.Models.PayoutSessionDto.PayoutSessionClient;
using BackgroundBank131Service.Repository.IRepository;
using BackgroundBank131Service.Validations;

namespace BackgroundBank131Service.Services;

public class SessionProcessingService
{
    private readonly ILogger<SessionProcessingService> _logger;
    private readonly IBank131Repository _bank131Repository;
    private readonly IBank131Client _bank131Client;
    private readonly IValidationStorage _validationStorage;
    
    public SessionProcessingService(ILogger<SessionProcessingService> logger, IBank131Repository bank131Repository, 
        IBank131Client bank131Client, IValidationStorage validationStorage)
    {
        _logger = logger;
        _bank131Repository = bank131Repository;
        _bank131Client = bank131Client;
        _validationStorage = validationStorage;
    }

    #region Actions

    public async Task ProcessPendingSessionsConfirmOrCancel(CancellationToken ct)
    {
        var sessions = await _bank131Repository.GetSessionsForProcessingAsync(ct);
        
        _logger.LogInformation("[{Prefix}] Found {Count} sessions to process", Constants.LogPrefixBackground, sessions.Count);

        var payoutSessionClient = new PayoutSessionClient();
        
        foreach (var session in sessions)
        {
            try
            {
                payoutSessionClient.SessionId = session.SessionId;

                _logger.LogInformation("[{Prefix}] Sending to Bank131 From Background Job: Session Id = {@SessionId}",
                    Constants.LogPrefixBackground, payoutSessionClient.SessionId);

                if (session.NextAction != null)
                {
                    var (isValid, response) = await _bank131Client.SendPaymentSessionAsync(payoutSessionClient, 2, ct);
                    if (!isValid)
                    {
                        _logger.LogError("[{Prefix}] Failed to take session. Validation errors: {Errors}",
                            Constants.LogPrefixBackground,
                            _validationStorage.GetError());
                    }
                }
                else
                {
                    var (isValid, response) = await _bank131Client.SendPaymentSessionAsync(payoutSessionClient, 3, ct);
                    if (!isValid)
                    {
                        _logger.LogError("[{Prefix}] Failed to take session. Validation errors: {Errors}",
                            Constants.LogPrefixBackground,
                            _validationStorage.GetError());
                    }
                }
                
                await _bank131Repository.MarkSessionAsConfirmedAsync(session.SessionId, ct);
                
                // Логирование результата
                _logger.LogInformation("[{Prefix}] Successfully confirmed session for the Final step: SessionId: {SessionId}",
                    Constants.LogPrefixBackground,
                    session.SessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming session {SessionId}", session.SessionId);
            }
        }
        
    }

    #endregion
}