using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

public interface ITokenService
{
    Task<PlexToken> GetToken(CancellationToken cancellationToken);
    Task<PlexToken> ValidateToken(CancellationToken cancellationToken);
}

class TokenService : ITokenService
{
    private readonly IStorageAdapter _storage;
    private readonly IPlexSdk _plex;

    public TokenService(IStorageAdapter storage, IPlexSdk plex)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _plex = plex ?? throw new ArgumentNullException(nameof(plex));
    }
    public async Task<PlexToken> GetToken(CancellationToken cancellationToken)
    {
        var infoFromCache = await _storage.RetrieveObject<PlexToken>(cancellationToken).ConfigureAwait(false);

        return infoFromCache switch
        {
            StorageReadResult<PlexToken>.Success result => result.Data,
            StorageReadResult<PlexToken>.Failure => new PlexToken(),
            _ => throw new ArgumentOutOfRangeException(nameof(infoFromCache))
        };
    }

    public Task<PlexToken> ValidateToken(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}