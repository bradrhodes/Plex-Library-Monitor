using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Subscription
    {
        public bool active { get; set; }
        public DateTime subscribedAt { get; set; }
        public string status { get; set; } = string.Empty;
        public string paymentService { get; set; } = string.Empty;
        public string plan { get; set; } = string.Empty;
        public List<string> features { get; set; } = new();
    }
}