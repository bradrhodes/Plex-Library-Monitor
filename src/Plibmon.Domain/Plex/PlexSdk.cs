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
                PinCode = response.Content.code,
                PinId = response.Content.id.ToString()
            };
        }

        public async Task<PollPinResponse> PollForPin(string pinId, string pinCode, string clientId)
        {
            var response = await _plexApi.PollForPin(pinId, pinCode, clientId).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return new PollPinResponse.PinInvalidOrExpired();

            if (response.Content == null)
            {
                _logger.LogError("Unable to deserialize PIN response");
                return new PollPinResponse.PinInvalidOrExpired();
            }

            if (string.IsNullOrEmpty(response.Content.authToken))
            {
                return new PollPinResponse.PinNotYetAuthorized();
            }

            return new PollPinResponse.Success
            {
                AuthToken = response.Content.authToken
            };
        }
    }
}