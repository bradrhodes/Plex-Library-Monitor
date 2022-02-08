using Plibmon.Shared;

namespace Plibmon.Domain;

class PlibmonService : IPlibmonService
{
    private readonly PlibmonSettings _settings;
    private readonly ITokenService _tokenService;
    private readonly IPinService _pinService;

    public PlibmonService(PlibmonSettings settings, ITokenService tokenService, IPinService pinService)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _pinService = pinService ?? throw new ArgumentNullException(nameof(pinService));
    }
    public async Task<bool> CanConnectToPlex(CancellationToken cancellationToken)
    {
        var plexToken = await _tokenService.GetToken(cancellationToken).ConfigureAwait(false);
        return !string.IsNullOrEmpty(plexToken.Token);
    }

    public Task<PinLinkResult> GetPinLink(CancellationToken cancellationToken)
        => _pinService.GetPinLink(_settings.ClientId, _settings.ClientName, cancellationToken);

    public async Task PollForPinAuthorization(CancellationToken cancellationToken)
    {
        var pinResult = _pinService.ValidatePin()
    }
}