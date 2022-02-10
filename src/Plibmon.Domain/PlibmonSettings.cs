namespace Plibmon.Domain
{
    public class PlibmonSettings
    {
        public const string ConfigSectionName = "Plibmon";
        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string CacheFolder { get; set; } = string.Empty;
        public string CacheFile { get; set; } = string.Empty;
        public string PlexApiBaseAddress { get; set; } = string.Empty;
    }
}