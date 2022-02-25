namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract record PinAuthorizationResponse
    {
        public record Success(PlexToken AuthToken) : PinAuthorizationResponse;

        public record PinNotYetAuthorized : PinAuthorizationResponse;

        public record PinAuthorizationInvalidOrExpired : PinAuthorizationResponse;
    }
}