namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract record ValidateTokenResponse
    {
        public record ValidToken : ValidateTokenResponse
        {
            public PlexUserInfo PlexUserInfo { get; set; } = new();
        }
        
        public record InvalidToken : ValidateTokenResponse;
    }
}