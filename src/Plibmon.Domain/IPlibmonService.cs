namespace Plibmon.Domain;

public interface IPlibmonService
{
    Task<bool> CanConnectToPlex(CancellationToken cancellationToken);
}