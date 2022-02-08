using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Plibmon.Domain.Plex.DomainModels;
using Plibmon.Domain.Plex.PlexModels;

namespace Plibmon.Domain.Plex
{
    public class PlexSdk : IPlexSdk
    {
        private readonly IPlexApi _plexApi;
        private readonly ILogger<PlexSdk> _logger;

        public PlexSdk(IPlexApi plexApi, ILogger<PlexSdk> logger)
        {
            _plexApi = plexApi ?? throw new ArgumentNullException(nameof(plexApi));
            _logger = logger;
        }
        
        public async Task<ValidateTokenResponse> ValidateToken(string token, string clientId, string clientName)
        {
            var response = await _plexApi.ValidateAuthToken(clientName, clientId, token).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode) return new ValidateTokenResponse.InvalidToken();
            
            if (response.Content == null)
                return new ValidateTokenResponse.ValidToken
                {
                    PlexUserInfo = new PlexUserInfo()
                };

            return new ValidateTokenResponse.ValidToken
            {
                PlexUserInfo = new PlexUserInfo
                {
                    Id = response.Content.id,
                    Thumbnail = response.Content.thumb,
                    Title = response.Content.title,
                    Username = response.Content.username,
                    Uuid = response.Content.uuid
                }
            };
        }

        public async Task<GetPinResponse> GetPin(string clientId, string clientName)
        {
            var response = await _plexApi.GetPin(clientName, clientId).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<Error>(response.Error?.Content);
                _logger.LogError("Unable to get PIN", error);
                return new GetPinResponse.Failure { Message = error.message };
            }
            if (response.Content == null)
            {
                _logger.LogError("Unable to deserialize PIN response");
                return new GetPinResponse.Failure { Message = "Unable to deserialize PIN response" };
            }

            return new GetPinResponse.Success()
            {
                PinInfo = new PinInfo(
                    PinId: response.Content.id.ToString(),
                    PinCode: response.Content.code)
            };
        }

        public async Task<PinAuthorizationResponse> CheckForPinAuthorization(string pinId, string pinCode, string clientId, CancellationToken cancellationToken)
        {
            var response = await _plexApi.CheckForPinAuthorization(pinId, pinCode, clientId).ConfigureAwait(false);
            _logger.LogDebug("CheckPin Response: {@response}", response);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Pin invalid or expired");
                return new PinAuthorizationResponse.PinAuthorizationInvalidOrExpired();
            }

            if (response.Content == null)
            {
                _logger.LogError("Unable to deserialize PIN response");
                return new PinAuthorizationResponse.PinAuthorizationInvalidOrExpired();
            }

            if (string.IsNullOrEmpty(response.Content.authToken))
            {
                _logger.LogInformation("Pin not authorized yet");
                return new PinAuthorizationResponse.PinNotYetAuthorized();
            }

            var token = new PlexToken(response.Content.authToken);
            _logger.LogDebug("Auth token received: {@token}", token);
            _logger.LogInformation("Auth token received");
            return new PinAuthorizationResponse.Success(token);
        }
    }
}