using Microsoft.Extensions.Options;
using Plibmon.Domain.Persistance;
using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

class TokenService : ITokenService
{
    private readonly IStorageAdapter _storage;
    private readonly IPlexSdk _plex;
    private readonly PlibmonSettings _settings;
    private readonly IClientIdService _clientIdService;

    public TokenService(IStorageAdapter storage, IPlexSdk plex, PlibmonSettings settings, 
        IClientIdService clientIdService)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _plex = plex ?? throw new ArgumentNullException(nameof(plex));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        _clientIdService = clientIdService ?? throw new ArgumentNullException(nameof(clientIdService));
    }
    public async Task<PlexToken> GetToken(CancellationToken cancellationToken)
    {
        var infoFromCache = await _storage.RetrieveObject<PlexToken>(cancellationToken).ConfigureAwait(false);

        return infoFromCache switch
        {
            StorageReadResult<PlexToken>.Success result => result.Data,
            StorageReadResult<PlexToken>.Failure => new PlexToken(string.Empty),
            _ => throw new ArgumentOutOfRangeException(nameof(infoFromCache))
        };
    }


    public async Task<bool> HaveValidToken(CancellationToken cancellationToken)
    {
        var token = await GetToken(cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrEmpty(token.Token))
            return false;

        var clientId = await _clientIdService.GetClientId(cancellationToken).ConfigureAwait(false);

        var tokenValidationResponse = await _plex.ValidateToken(token.Token,
            clientId.ClientId, _settings.ClientName).ConfigureAwait(false);

        return tokenValidationResponse switch
        {
            ValidateTokenResponse.ValidToken => true,
            ValidateTokenResponse.InvalidToken => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}