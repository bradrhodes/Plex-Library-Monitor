using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Microsoft.Extensions.Logging;
using Plibmon.Domain.Events;
using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;
using Plibmon.Shared;

namespace Plibmon.Domain;

public interface IPinService
{
    Task<PinLinkResult> GetPinLink(string clientId, string clientName, CancellationToken cancellationToken);
    Task<ValidatePinResult> ValidatePin(string pinId, string pinCode, string clientId, CancellationToken cancellationToken);
}

class PinService : IPinService
{
    private readonly IPlexSdk _plex;
    private readonly IGenerateAuthAppUrl _linkGenerator;
    private readonly IStorageAdapter _storage;
    private readonly ILogger<PinService> _logger;

    public PinService(IPlexSdk plex, IGenerateAuthAppUrl linkGenerator, 
        IStorageAdapter storage, ILogger<PinService> logger, IBackgroundJobManager)
    {
        _plex = plex ?? throw new ArgumentNullException(nameof(plex));
        _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<PinLinkResult> GetPinLink(string clientId, string clientName, CancellationToken cancellationToken)
    {
        var getPinResult = await _plex.GetPin(clientId, clientName).ConfigureAwait(false);

        return getPinResult switch
        {
            GetPinResponse.Failure f => new PinLinkResult.PinLinkFailure(f.Message),
            GetPinResponse.Success s => new PinLinkResult.PinLinkSuccess(
                await StoreAndGenerateLink(clientId, clientName, s.PinInfo, cancellationToken)),
            _ => throw new ArgumentOutOfRangeException(nameof(getPinResult))
        };
    }

    private async Task<string> StoreAndGenerateLink(string clientId, string clientName, PinInfo pinInfo, CancellationToken cancellationToken)
    {
        var storeResult = await _storage.StoreObject(pinInfo, cancellationToken).ConfigureAwait(false);

        switch (storeResult)
        {
            case StorageWriteResult.Success:
                _logger.LogInformation("PinInfo stored successfully.");
                break;
            case StorageWriteResult.Failure:
                _logger.LogInformation("Failed to store PinInfo");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Trigger polling for a result
        return _linkGenerator.GenerateUrl(clientId, pinInfo.PinCode, clientName);
    }

    // Todo: this method is meant to be called in a fire and forget fashion
    public async Task<ValidatePinResult> ValidatePin(string pinId, string pinCode, string clientId)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var pinAuthorizationResponse = await _plex
                .CheckForPinAuthorization(pinId: pinId, pinCode: pinCode, clientId: clientId, cancellationToken)
                .ConfigureAwait(false);

            switch (pinAuthorizationResponse)
            {
                case PinAuthorizationResponse.Success s:
                    // Store the result
                    // Todo: figure out how to propagate this event back out to the UI
                    //  - perhaps use a backgroundService that always checks for this value, then uses
                    //    signal r to send that to the UI
                    switch (await _storage.StoreObject(s.AuthToken, cancellationToken).ConfigureAwait(false))
                    {
                        case StorageWriteResult.Success:
                            _logger.LogInformation("Successfully stored PlexToken.");
                            break;
                        case StorageWriteResult.Failure:
                            _logger.LogError("Unable to store PlexToken.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    cancellationTokenSource.Cancel();
                    break;
                case PinAuthorizationResponse.PinNotYetAuthorized:
                    break;
                case PinAuthorizationResponse.PinAuthorizationInvalidOrExpired:
                    break;
                default:
                    break;

            }

        }
    }

    interface IValidateAppAuthorization
    {
        Task CheckForAuthorization(string pinId, string pinCode, string clientId, CancellationToken cancellationToken);
    }
    
    public class AppAuthorizationValidator : IValidateAppAuthorization
    {
        private readonly IPlexSdk _plex;
        private readonly IMediator _mediator;
        private readonly IStorageAdapter _storage;
        private readonly ILogger<AppAuthorizationChecked> _logger;

        public AppAuthorizationValidator(IPlexSdk plex, IMediator mediator, IStorageAdapter storage, ILogger<AppAuthorizationChecked> logger)
        {
            _plex = plex ?? throw new ArgumentNullException(nameof(plex));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task CheckForAuthorization(string pinId, string pinCode, string clientId, CancellationToken cancellationToken)
        {
            var pinAuthorizationResponse = await _plex
                .CheckForPinAuthorization(pinId: pinId, pinCode: pinCode, clientId: clientId, CancellationToken.None)
                .ConfigureAwait(false);

            switch (pinAuthorizationResponse)
            {
                case PinAuthorizationResponse.Success s:
                    // Store the result
                    // Todo: figure out how to propagate this event back out to the UI
                    //  - perhaps use a backgroundService that always checks for this value, then uses
                    //    signal r to send that to the UI
                    switch (await _storage.StoreObject(s.AuthToken, cancellationToken).ConfigureAwait(false))
                    {
                        case StorageWriteResult.Success:
                            _logger.LogInformation("Successfully stored PlexToken.");
                            break;
                        case StorageWriteResult.Failure:
                            _logger.LogError("Unable to store PlexToken.");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    cancellationTokenSource.Cancel();
                    break;
                case PinAuthorizationResponse.PinNotYetAuthorized:
                    break;
                case PinAuthorizationResponse.PinAuthorizationInvalidOrExpired:
                    break;
                default:
                    break;

            }
            
            private Task 
        }
    }
}