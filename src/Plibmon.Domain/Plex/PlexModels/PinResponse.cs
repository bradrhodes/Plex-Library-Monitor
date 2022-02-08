namespace Plibmon.Domain.Plex.PlexModels
{
    public class PinResponse
    {
        public long id { get; set; }
        public string code { get; set; }
        public string product { get; set; }
        public bool trusted { get; set; }
        public string clientIdentifier { get; set; }
        public Location location { get; set; }
        public int expiresIn { get; set; }
        public string createdAt { get; set; }
        public string expiresAt { get; set; }
        public string authToken { get; set; }
        public bool? newRegistration { get; set; }
    }
}