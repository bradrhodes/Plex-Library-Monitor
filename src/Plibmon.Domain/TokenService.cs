using Plibmon.Domain.Persistance;
using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

class TokenService : ITokenService
{
    private readonly IStorageAdapter _storage;
    private readonly IPlexSdk _plex;
    private readonly PlibmonSettings _settings;

    public TokenService(IStorageAdapter storage, IPlexSdk plex, PlibmonSettings settings)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _plex = plex ?? throw new ArgumentNullException(nameof(plex));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
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

        var tokenValidationResponse = await _plex.ValidateToken(token.Token,
            _settings.ClientId, _settings.ClientName).ConfigureAwait(false);

        return tokenValidationResponse switch
        {
            ValidateTokenResponse.ValidToken => true,
            ValidateTokenResponse.InvalidToken => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

}