namespace Plibmon.Domain.Plex.PlexModels
{
    public class Subscription
    {
        public bool active { get; set; }
        public DateTime subscribedAt { get; set; }
        public string status { get; set; }
        public string paymentService { get; set; }
        public string plan { get; set; }
        public List<string> features { get; set; }
    }
}