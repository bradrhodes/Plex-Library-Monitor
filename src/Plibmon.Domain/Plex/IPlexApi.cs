using Plibmon.Domain.Plex.PlexModels;
using Refit;

namespace Plibmon.Domain.Plex
{
    public interface IPlexApi
    {
        // [Post("pins?X-Plex-Product={clientName}&X-Plex-Client-Identifier={clientId}&strong=true")]
        [Post("/pins")]
        Task<ApiResponse<PinResponse>> GetPin([AliasAs("X-Plex-Product")] string clientName, 
            [AliasAs("X-Plex-Client-Identifier")] string clientId,
            [AliasAs("strong")] bool strong = true);

        // https://plex.tv/api/v2/pins/{{PinId}}?code={{PinCode}}&X-Plex-Client-Identifier={{ClientId}}
        [Get("/pins/{pinId}")]
        Task<ApiResponse<PinResponse>> PollForPin([AliasAs("pinId")] string pinId,
            [AliasAs("code")] string pinCode, 
            [AliasAs("X-Plex-Client-Identifier")] string clientId);

        // https://plex.tv/api/v2/user?X-Plex-Product={{ClientName}}&X-Plex-Product-Identifier={{ClientId}}&X-Plex-Token={{ClientAuthToken}}
        [Get("/user")]
        Task<ApiResponse<AuthCheckResponse>> ValidateAuthToken(
            [AliasAs("X-Plex-Product")] string clientName,
            [AliasAs("X-Plex-Product-Identifier")] string clientId,
            [AliasAs("X-Plex-Token")] string token);
    }
    
    
}