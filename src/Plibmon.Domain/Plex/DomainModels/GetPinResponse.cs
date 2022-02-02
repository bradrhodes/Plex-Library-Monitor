namespace Plibmon.Domain.Plex.DomainModels
{
    public abstract class GetPinResponse
    {
        public class Success : GetPinResponse
        {
            public string PinId { get; set; }
            public string PinCode { get; set; }
        }

        public class Failure : GetPinResponse
        {
            public string Message { get; set; }
        }
    }
}