using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Profile
    {
        public bool autoSelectAudio { get; set; }
        public string defaultAudioLanguage { get; set; } = string.Empty;
        public string defaultSubtitleLanguage { get; set; } = string.Empty;
        public int autoSelectSubtitle { get; set; }
        public int defaultSubtitleAccessibility { get; set; }
        public int defaultSubtitleForced { get; set; }
    }
}