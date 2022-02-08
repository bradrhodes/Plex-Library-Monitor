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
        // Get PinInfo
        var pinInfo = await _storage.RetrieveObject<PinInfo>(cancellationToken).ConfigureAwait(false) switch
        {
            StorageReadResult<PinInfo>.Success s => s.Data,
            StorageReadResult<PinInfo>.Failure => new PinInfo(string.Empty, string.Empty),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (string.IsNullOrEmpty(pinInfo.PinCode) || string.IsNullOrEmpty(pinInfo.PinId))
            return new PinAuthorizationResponse.PinAuthorizationInvalidOrExpired();
        
        var pinAuthorizationResponse = await _plex
            .CheckForPinAuthorization(pinId: pinInfo.PinId, pinCode: pinInfo.PinCode, clientId: clientId, cancellationToken)
            .ConfigureAwait(false);

        if (pinAuthorizationResponse is PinAuthorizationResponse.Success pin)
        {
            await _storage.StoreObject(pin.AuthToken, cancellationToken);
        }

        return pinAuthorizationResponse;
    }
}