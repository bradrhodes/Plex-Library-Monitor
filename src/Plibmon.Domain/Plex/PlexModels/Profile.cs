namespace Plibmon.Domain.Plex.PlexModels
{
    public class Profile
    {
        public bool autoSelectAudio { get; set; }
        public string defaultAudioLanguage { get; set; }
        public string defaultSubtitleLanguage { get; set; }
        public int autoSelectSubtitle { get; set; }
        public int defaultSubtitleAccessibility { get; set; }
        public int defaultSubtitleForced { get; set; }
    }
}