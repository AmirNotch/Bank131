using Bank131Connector.Clients.IClients;
using Bank131Connector.Models;
using Bank131Connector.Models.ActualCourse;
using Bank131Connector.Models.CreatingPaymentSessionDto;
using Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient;
using Bank131Connector.Models.db;
using Bank131Connector.Models.PaymentRequestDto;
using Bank131Connector.Models.WebhookPaymentFinishedRequest;
using Bank131Connector.Repository.IRepository;
using Bank131Connector.Validations;
using Bank131Connector.Utils;
using AmountDetails = Bank131Connector.Models.CreatingPaymentSessionDto.PaymentSessionClient.AmountDetails;
using Bank131Connector.Models.WebhookReadyToConfirmDto;
using Contact = Bank131Connector.Models.PaymentRequestDto.PaymentClient.Contact;

namespace Bank131Connector.Services;

public class Bank131Service
{
    private readonly IBank131Repository _bank131Repository;
    private readonly IBank131Client _bank131Client;
    private readonly IVaslClient _vaslClient;
    private readonly IValidationStorage _validationStorage;
    private readonly ILogger<Bank131Service> _logger;
    
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
        try
        {
            var generator = new IdGenerator(0);
            var id = generator.CreateId();

            var request = new PaymentSessionClientRequest
            {
                AmountDetails = new AmountDetails
                {
                    Amount = paymentSessionRequest.Amount,
                    Currency = paymentSessionRequest.Currency
                },
                Metadata = $"order{id}"
            };

            // Логируем входящий запрос (с маскировкой чувствительных данных)
            _logger.LogInformation("[{Prefix}] Starting processing for request: {@Request}",
                Constants.LogPrefixSession,
                new
                {
                    paymentSessionRequest.Amount,
                    paymentSessionRequest.Currency,
                    CardNumber = MaskCardNumber(paymentSessionRequest.CardNumber),
                    request.Metadata
                });

            _logger.LogInformation("[{Prefix}] Sending to Bank131: {@Request}", Constants.LogPrefixSession, request);

            var (isValidSession, response) = await _bank131Client.SendPaymentSessionAsync(request, ct);
            if (!isValidSession)
            {
                _logger.LogError("[{Prefix}] Failed to create payment session. Validation errors: {Errors}",
                    Constants.LogPrefixSession,
                    _validationStorage.GetError());
                return false;
            }

            var session = new Bank131Session
            {
                SessionId = response.Session.Id,
                Status = response.Session.Status,
                CreatedAt = response.Session.CreatedAt,
                UpdatedAt = response.Session.UpdatedAt,
                Amount = request.AmountDetails.Amount,
                Currency = request.AmountDetails.Currency,
                Metadata = request.Metadata,
                NextAction = Constants.NextAction,
                InitCardNumber = paymentSessionRequest.CardNumber,
                InitCardType = Constants.InitCardType
            };

            await _bank131Repository.CreateSession(session, ct);
            
            // Детальное логирование результата
            _logger.LogInformation("""
                                   [{Prefix}] Successfully created session:
                                   SessionId: {SessionId}
                                   Status: {Status}
                                   Amount: {Amount} {Currency}
                                   Metadata: {Metadata}
                                   Card: {CardMask}
                                   Full session: {@Session}
                                   """,
                Constants.LogPrefixSession,
                session.SessionId,
                session.Status,
                session.Amount,
                session.Currency,
                session.Metadata,
                MaskCardNumber(session.InitCardNumber),
                session); // Сериализация через @

            var payout = new PaymentRequest
            {
                SessionId = response.Session.Id,
                Metadata = request.Metadata,
                PaymentMethod = new PaymentMethod()
                {
                    Type = Constants.PaymentMethodType,
                    Card = new Card
                    {
                        Type = Constants.CardType,
                        BankCard = new BankCard
                        {
                            Number = paymentSessionRequest.CardNumber
                        }
                    }
                },
                ParticipantDetails = new ParticipantDetails
                {
                    Recipient = new Recipient
                    {
                        FullName = paymentSessionRequest.FullName
                    }
                },
                AmountDetails = new Models.PaymentRequestDto.AmountDetails
                {
                    Amount = paymentSessionRequest.Amount,
                    Currency = paymentSessionRequest.Currency
                }
            };
            
            _logger.LogInformation("[{Prefix}] Sending to Bank131: {@Request}", Constants.LogPrefixPayout, payout);
            
            var (isValidPayout, responsePayout) = await _bank131Client.SendPayoutAsync(payout);
            if (!isValidPayout)
            {
                _logger.LogError("[{Prefix}] Failed to create payment session. Validation errors: {Errors}",
                    Constants.LogPrefixPayout,
                    _validationStorage.GetError());
                return false;
            }
            
            _logger.LogInformation("[{Prefix}] Payout processed successfully. Payout response: {responsePayout}",
                Constants.LogPrefixPayout,
                responsePayout);

            var payoutEntity = new Bank131Payment
            {
                PaymentId = responsePayout.Session.PayoutLists.Select(p => p.Id).FirstOrDefault(),
                SessionId = responsePayout.Session.Id,
                Status = responsePayout.Session.Status,
                Amount = request.AmountDetails.Amount,
                FeeAmount = null,
                CardLast4 = responsePayout.Session.PayoutLists
                    .Select(p => p.PayoutDetails?.Card?.Last4)
                    .FirstOrDefault(),
                CardBrand = responsePayout.Session.PayoutLists
                    .Select(p => p.PayoutDetails?.Card?.Brand)
                    .FirstOrDefault(),
                RecipientName = paymentSessionRequest.FullName,
                RecipientEmail = responsePayout.Session.PayoutLists
                    .SelectMany(p => p.Customer?.Contacts ?? Enumerable.Empty<Contact>())
                    .Select(c => c.Email)
                    .FirstOrDefault()
            };
            
            await _bank131Repository.CreatePayout(payoutEntity, ct);
            _logger.LogInformation("""
                                   [{Prefix}] Successfully created payout :
                                   PaymentId: {SessionId}
                                   SessionId: {Status}
                                   Status: {Amount}
                                   Amount: {Metadata}
                                   CardLast4: {CardMask}
                                   CardBrand: {@Session}
                                   RecipientName: {Amount}
                                   RecipientEmail: {Metadata}
                                   Full payout: {@Session}
                                   """,
                Constants.LogPrefixPayout,
                payoutEntity.PaymentId,
                payoutEntity.SessionId,
                payoutEntity.Status,
                payoutEntity.Amount,
                payoutEntity.CardLast4,
                payoutEntity.CardBrand,
                payoutEntity.RecipientName,
                payoutEntity.RecipientEmail,
                payoutEntity); // Сериализация через @

            return true;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("[{Prefix}] Operation was cancelled", Constants.LogPrefixSession);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Prefix}] Unexpected error while creating payment session", Constants.LogPrefixSession);
            return false;
        }
    }
    
    public async Task<bool> ReadyToConfirm(WebhookReadyToConfirmRequest webhookReadyToConfirmRequest, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("[{Prefix}] Starting processing for Updating Entity: {@Request}",
                Constants.ReadyToConfirm, webhookReadyToConfirmRequest);

            _logger.LogInformation("[{Prefix}] Updating Session and Payout to ready_confirm status",
                Constants.ReadyToConfirm);

            await _bank131Repository.UpdatingSessionAndPayout(webhookReadyToConfirmRequest, ct);


            // Детальное логирование результата
            _logger.LogInformation("""
                                   [{Prefix}] Successfully updated payout:
                                   Updated payout and session: {@Payout}
                                   """,
                Constants.ReadyToConfirm,
                webhookReadyToConfirmRequest); // Сериализация через @
            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Prefix}] Unexpected error while creating payment session",
                Constants.ReadyToConfirm);
            return false;
        }
    }

    public async Task<bool> PaymentFinished(WebhookPaymentFinishedRequest webhookPaymentFinishedRequest, CancellationToken ct)
    {
        try
        {
            bool isValid = ValidateSessionStatus(webhookPaymentFinishedRequest.Type, 
                webhookPaymentFinishedRequest.Session.PayoutList.Select(p => p.Id).FirstOrDefault(), 
                webhookPaymentFinishedRequest.Session.Id, ct);
            if (!isValid)
            {
                return false;
            }
            
            _logger.LogInformation("[{Prefix}] Starting processing for Updating Entities: {@Request}",
                Constants.PaymentFinished, webhookPaymentFinishedRequest);

            _logger.LogInformation("[{Prefix}] Updating Session and Payout to payment_finished status", Constants.PaymentFinished);
            
            await _bank131Repository.PaymentFinishedUpdatingSessionAndPayout(webhookPaymentFinishedRequest, ct);


            // Детальное логирование результата
            _logger.LogInformation("""
                                   [{Prefix}] Successfully updated payout:
                                   Updated payout and session: {@Payout}
                                   """,
                Constants.PaymentFinished,
                webhookPaymentFinishedRequest); // Сериализация через @
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{Prefix}] Unexpected error while creating payment session", Constants.ReadyToConfirm);
            return false;
        }
    }

    // Метод для маскировки номера карты
    private static string MaskCardNumber(string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 8)
            return "****";
    
        return $"{cardNumber[..4]}****{cardNumber[^4..]}";
    }
    
    #endregion
    
    #region Validations

    private bool ValidateSessionStatus(string sessionType, string? paymentId, string? sessionId, CancellationToken ct)
    {
        if (!sessionType.Equals(Constants.PaymentFinished))
        {
            ValidationUtils.AddSessionTypeError(_validationStorage, sessionType, paymentId, sessionId);
        }
        return _validationStorage.IsValid;
    }
    
    #endregion
}