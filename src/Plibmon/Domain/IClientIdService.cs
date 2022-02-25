using Microsoft.Extensions.Logging;
using Plibmon.Domain.Persistance;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

public interface IClientIdService
{
    Task<PlexClientId> GetClientId(CancellationToken cancellationToken);
}

class ClientIdService : IClientIdService
{
    private readonly ILogger<ClientIdService> _logger;
    private readonly IStorageAdapter _storage;

    public ClientIdService(ILogger<ClientIdService> logger, IStorageAdapter storage)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async Task<PlexClientId> GetClientId(CancellationToken cancellationToken)
    {
        var infoFromCache = await _storage.RetrieveObject<PlexClientId>(cancellationToken).ConfigureAwait(false);
        
        // If it doesn't exist, generate it
        PlexClientId result;
        switch (infoFromCache)
        {
            case StorageReadResult<PlexClientId>.Success s:
                result = s.Data;
                break;
            case StorageReadResult<PlexClientId>.Failure:
                result = new PlexClientId(Guid.NewGuid().ToString());
                await _storage.StoreObject(result, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(infoFromCache));
        }

        return result;
    }
}
