using System.Diagnostics.CodeAnalysis;

namespace Plibmon.Domain.Plex.PlexModels
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AuthCheckResponse
    {
        public int id { get; set; } = default;
        public string uuid { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string friendlyName { get; set; } = string.Empty;
        public string locale { get; set; } = string.Empty;
        public bool confirmed { get; set; }
        public bool emailOnlyAuth { get; set; }
        public bool hasPassword { get; set; }
        public bool @protected { get; set; }
        public string thumb { get; set; } = string.Empty;
        public string authToken { get; set; } = string.Empty;
        public string mailingListStatus { get; set; } = string.Empty;
        public bool mailingListActive { get; set; }
        public string scrobbleTypes { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public Subscription subscription { get; set; } = new();
        public string subscriptionDescription { get; set; } = string.Empty;
        public bool restricted { get; set; }
        public string anonymous { get; set; } = string.Empty;
        public bool home { get; set; }
        public bool guest { get; set; }
        public int homeSize { get; set; }
        public bool homeAdmin { get; set; }
        public int maxHomeSize { get; set; }
        public int certificateVersion { get; set; }
        public int rememberExpiresAt { get; set; }
        public Profile profile { get; set; } = new();
        public List<string> entitlements { get; set; } = new();
        public List<string> roles { get; set; } = new();
        public List<Service> services { get; set; } = new();
        public string adsConsent { get; set; } = string.Empty;
        public string adsConsentSetAt { get; set; } = string.Empty;
        public string adsConsentReminderAt { get; set; } = string.Empty;
        public bool experimentalFeatures { get; set; }
        public bool twoFactorEnabled { get; set; }
        public bool backupCodesCreated { get; set; }
    }
}