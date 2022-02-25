using Microsoft.Extensions.Logging;
using Plibmon.Domain.Persistance;
using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;
using Plibmon.Shared;

namespace Plibmon.Domain;

class PinService : IPinService
{
    private readonly IPlexSdk _plex;
    private readonly IGenerateAuthAppUrl _linkGenerator;
    private readonly IStorageAdapter _storage;
    private readonly ILogger<PinService> _logger;

    public PinService(IPlexSdk plex, IGenerateAuthAppUrl linkGenerator, 
        IStorageAdapter storage, ILogger<PinService> logger)
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
        await _storage.StoreObject(pinInfo, cancellationToken).ConfigureAwait(false);
        return _linkGenerator.GenerateUrl(clientId, pinInfo.PinCode, clientName);
    }

    public async Task<PinAuthorizationResponse> ValidatePin(string clientId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Attempting to find Pin in storage");
        // Get PinInfo
        var pinInfo = await _storage.RetrieveObject<PinInfo>(cancellationToken).ConfigureAwait(false) switch
        {
            StorageReadResult<PinInfo>.Success s => s.Data,
            StorageReadResult<PinInfo>.Failure => new PinInfo(string.Empty, string.Empty),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        _logger.LogDebug("PinInfo from storage: {@pinInfo}", pinInfo);

        if (string.IsNullOrEmpty(pinInfo.PinCode) || string.IsNullOrEmpty(pinInfo.PinId))
        {
            _logger.LogDebug("No PinInfo from storage. Cannot validate.");
            return new PinAuthorizationResponse.PinAuthorizationInvalidOrExpired();
        }
        
        _logger.LogDebug("Checking Authorization.");
        var pinAuthorizationResponse = await _plex
            .CheckForPinAuthorization(pinId: pinInfo.PinId, pinCode: pinInfo.PinCode, clientId: clientId, cancellationToken)
            .ConfigureAwait(false);
        _logger.LogDebug("Authorization response received: {@authResponse}", pinAuthorizationResponse);

        if (pinAuthorizationResponse is PinAuthorizationResponse.Success pin)
        {
            _logger.LogInformation("Pin was authorized.");
            await _storage.StoreObject(pin.AuthToken, cancellationToken).ConfigureAwait(false);
            await _storage.RemoveObject<PinInfo>(cancellationToken).ConfigureAwait(false);
        }

        _logger.LogInformation("Pin was not Authorized");
        return pinAuthorizationResponse;
    }
}