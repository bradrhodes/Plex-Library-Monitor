namespace Plibmon.Domain;

class PlibmonService : IPlibmonService
{
    private readonly ITokenService _tokenService;

    public PlibmonService(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }
    public async Task<bool> CanConnectToPlex(CancellationToken cancellationToken)
    {
        var plexToken = await _tokenService.GetToken(cancellationToken).ConfigureAwait(false);
        return !string.IsNullOrEmpty(plexToken.Token);
    }
}