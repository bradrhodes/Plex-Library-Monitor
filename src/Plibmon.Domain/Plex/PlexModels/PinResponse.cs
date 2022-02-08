using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PinResponse
    {
        public long id { get; set; }
        public string code { get; set; } = string.Empty;
        public string product { get; set; } = string.Empty;
        public bool trusted { get; set; }
        public string clientIdentifier { get; set; } = string.Empty;
        public Location location { get; set; } = new();
        public int expiresIn { get; set; }
        public string createdAt { get; set; } = string.Empty;
        public string expiresAt { get; set; } = string.Empty;
        public string authToken { get; set; } = string.Empty;
        public bool? newRegistration { get; set; }
    }
}