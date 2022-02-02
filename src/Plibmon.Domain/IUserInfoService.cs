using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain
{
    public interface IUserInfoService
    {
        Task<PlexUserInfo> GetUserInfo(CancellationToken cancellationToken);
        Task<PlexUserInfo> RefreshUserInfo(CancellationToken cancellationToken);
    }
}