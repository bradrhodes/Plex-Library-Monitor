using Plibmon.Shared;

namespace Plibmon.Domain;

public interface IPlibmonService
{
    Task<bool> CanConnectToPlex(CancellationToken cancellationToken);
    Task<PinLinkResult> GetPinLink(CancellationToken cancellationToken);
    
    Task ValidatePin()
}