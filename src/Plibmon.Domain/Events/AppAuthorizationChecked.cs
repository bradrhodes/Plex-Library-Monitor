using MediatR;
using Plibmon.Shared;

namespace Plibmon.Domain.Events;

public abstract record AppAuthorizationChecked : INotification
{
    public record Authorized() : AppAuthorizationChecked;

    public record NotAuthorized() : AppAuthorizationChecked;

    public record PinNotFound() : AppAuthorizationChecked;
}