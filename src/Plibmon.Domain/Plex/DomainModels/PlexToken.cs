namespace Plibmon.Domain.Plex.DomainModels
{
    public record PlexToken()
    {
        public string Token { get; init; } = string.Empty;
    }
}