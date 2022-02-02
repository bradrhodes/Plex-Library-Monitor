using Plibmon.Domain.Plex.DomainModels;

namespace Plibmon.Domain.Plex
{
    public interface IPlexSdk
    {
        Task<ValidateTokenResponse> ValidateToken(string token, string clientId, string clientName);
        Task<GetPinResponse> GetPin(string clientId, string clientName);
        Task<PollPinResponse> PollForPin(string pinId, string pinCode, string clientId);
    }
}