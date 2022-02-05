using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;
using Plibmon.Shared;

namespace Plibmon.Domain;

public interface IPinService
{
    Task<PinLinkResult> GetPinLink(string clientId, string clientName, CancellationToken cancellationToken);
    Task<ValidatePinResult> ValidatePin(CancellationToken cancellationToken);
}

class PinService : IPinService
{
    private readonly IPlexSdk _plex;
    private readonly IGenerateAuthAppUrl _linkGenerator;

    public PinService(IPlexSdk plex, IGenerateAuthAppUrl linkGenerator)
    {
        _plex = plex ?? throw new ArgumentNullException(nameof(plex));
        _linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
    }


    public async Task<PinLinkResult> GetPinLink(string clientId, string clientName, CancellationToken cancellationToken)
    {
        var getPinResult = await _plex.GetPin(clientId, clientName).ConfigureAwait(false);

        return getPinResult switch
        {
            GetPinResponse.Failure f => new PinLinkResult.PinLinkFailure(f.Message),
            GetPinResponse.Success s => new PinLinkResult.PinLinkSuccess(
                _linkGenerator.GenerateUrl(clientId, s.PinCode, clientName)),
            _ => throw new ArgumentOutOfRangeException(nameof(getPinResult))
        };
    }

    public Task<ValidatePinResult> ValidatePin(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}