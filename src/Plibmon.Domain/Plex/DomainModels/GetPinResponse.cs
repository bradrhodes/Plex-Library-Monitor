namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract class GetPinResponse
    {
        public class Success : GetPinResponse
        {
            public PinInfo PinInfo { get; init; }
        }

        public class Failure : GetPinResponse
        {
            public string Message { get; init; }
        }
    }
}