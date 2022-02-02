namespace Plibmon.Domain.Plex.PlexModels
{
    public class AuthCheckResponse
    {
        public int id { get; set; }
        public string uuid { get; set; }
        public string username { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string friendlyName { get; set; }
        public string locale { get; set; }
        public bool confirmed { get; set; }
        public bool emailOnlyAuth { get; set; }
        public bool hasPassword { get; set; }
        public bool @protected { get; set; }
        public string thumb { get; set; }
        public string authToken { get; set; }
        public string mailingListStatus { get; set; }
        public bool mailingListActive { get; set; }
        public string scrobbleTypes { get; set; }
        public string country { get; set; }
        public Subscription subscription { get; set; }
        public string subscriptionDescription { get; set; }
        public bool restricted { get; set; }
        public string anonymous { get; set; }
        public bool home { get; set; }
        public bool guest { get; set; }
        public int homeSize { get; set; }
        public bool homeAdmin { get; set; }
        public int maxHomeSize { get; set; }
        public int certificateVersion { get; set; }
        public int rememberExpiresAt { get; set; }
        public Profile profile { get; set; }
        public List<string> entitlements { get; set; }
        public List<string> roles { get; set; }
        public List<Service> services { get; set; }
        public string adsConsent { get; set; }
        public string adsConsentSetAt { get; set; }
        public string adsConsentReminderAt { get; set; }
        public bool experimentalFeatures { get; set; }
        public bool twoFactorEnabled { get; set; }
        public bool backupCodesCreated { get; set; }
    }
}