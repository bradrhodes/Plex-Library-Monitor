using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Location
    {
        public string code { get; set; } = string.Empty;
        public bool european_union_member { get; set; }
        public string continent_code { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string time_zone { get; set; } = string.Empty;
        public string postal_code { get; set; } = string.Empty;
        public bool in_privacy_restricted_country { get; set; }
        public string subdivisions { get; set; } = string.Empty;
        public string coordinates { get; set; } = string.Empty;
    }
}