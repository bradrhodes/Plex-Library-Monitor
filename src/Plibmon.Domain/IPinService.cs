using System.Runtime.InteropServices.ComTypes;
using MediatR;
using Plibmon.Domain.Events;
using Plibmon.Domain.Plex.DomainModels;
using Plibmon.Shared;

namespace Plibmon.Domain;

public interface IPinService
{
    Task<PinLinkResult> GetPinLink(string clientId, string clientName, CancellationToken cancellationToken);
    Task<PinAuthorizationResponse> ValidatePin(string clientId, CancellationToken cancellationToken);
}