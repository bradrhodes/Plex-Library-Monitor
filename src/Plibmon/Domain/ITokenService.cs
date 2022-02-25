using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

public interface ITokenService
{
    Task<PlexToken> GetToken(CancellationToken cancellationToken);
    Task<bool> HaveValidToken(CancellationToken cancellationToken);
}