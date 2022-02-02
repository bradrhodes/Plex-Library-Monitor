namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract class PollPinResponse
    {
        public class Success : PollPinResponse
        {
            public string AuthToken { get; set; }
        }

        public class PinNotYetAuthorized : PollPinResponse { }
        
        public class PinInvalidOrExpired : PollPinResponse{ }
    }
}