namespace PlexLibraryMonitor.Plex.DomainModels
{
    public class PlexUserInfo
    {
        public int Id { get; set; } = 0;
        public string Uuid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
    }
}