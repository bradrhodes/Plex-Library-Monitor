using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Service
    {
        public string identifier { get; set; } = string.Empty;
        public string endpoint { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string secret { get; set; } = string.Empty;
    }
}