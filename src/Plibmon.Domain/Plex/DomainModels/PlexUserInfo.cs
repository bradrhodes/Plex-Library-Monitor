namespace Plibmon.Domain.Plex.DomainModels
{
    public record PlexUserInfo
    {
        public int Id { get; init; } = default;
        public string Uuid { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Thumbnail { get; init; } = string.Empty;
    }
}