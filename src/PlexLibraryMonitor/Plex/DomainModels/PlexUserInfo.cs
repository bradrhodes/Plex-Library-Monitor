using System;

namespace PlexLibraryMonitor.Plex.DomainModels
{
    public record PlexUserInfo
    {
        public int Id { get; init; } 
        public string Uuid { get; init; } 
        public string Username { get; init; }
        public string Title { get; init; }
        public string Thumbnail { get; init; }
    }
}