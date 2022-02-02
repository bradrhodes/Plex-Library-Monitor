using Plibmon.Domain.Plex;
using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain;

class UserInfoService : IUserInfoService
{
    private readonly IStorageAdapter _storage;
    private readonly IPlexSdk _plexSdk;
    private readonly PlibmonSettings _settings;

    public UserInfoService(IStorageAdapter storage, IPlexSdk plexSdk, PlibmonSettings settings)
    {
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _plexSdk = plexSdk;
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }
    public async Task<PlexUserInfo> GetUserInfo(CancellationToken cancellationToken)
    {
        var infoFromCache = await _storage.RetrieveObject<PlexUserInfo>(cancellationToken).ConfigureAwait(false);

        return infoFromCache switch
        {
            StorageReadResult<PlexUserInfo>.Success result => result.Data,
            StorageReadResult<PlexUserInfo>.Failure => await RefreshUserInfo(cancellationToken).ConfigureAwait(false),
            _ => throw new ArgumentOutOfRangeException(nameof(infoFromCache))
        };
    }

    public async Task<PlexUserInfo> RefreshUserInfo(CancellationToken cancellationToken)
    {
        // Get the token
        var tokenData = await _storage.RetrieveObject<PlexToken>(cancellationToken).ConfigureAwait(false);
            
        // If no token, then can't communicate with Plex API
        if (tokenData is StorageReadResult<PlexToken>.Failure)
            return new PlexUserInfo();

        var token = (StorageReadResult<PlexToken>.Success)tokenData;
            
        // Call the validate token endpoint to get the user info
        var getUserInfoFromPlex = await _plexSdk.ValidateToken(
            token.Data.Token, _settings.ClientId, _settings.ClientName).ConfigureAwait(false);

        if (getUserInfoFromPlex is ValidateTokenResponse.InvalidToken)
            return new PlexUserInfo();

        var plexUserInfo = (ValidateTokenResponse.ValidToken)getUserInfoFromPlex;

        await SaveUserInfo(plexUserInfo.PlexUserInfo, cancellationToken);
        return plexUserInfo.PlexUserInfo;
    }

    private async Task SaveUserInfo(PlexUserInfo userInfo, CancellationToken cancellationToken)
    {
        await _storage.StoreObject(userInfo, cancellationToken).ConfigureAwait(false);
    }
}