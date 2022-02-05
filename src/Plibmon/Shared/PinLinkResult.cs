namespace Plibmon.Shared;

public abstract record PinLinkResult
{
    public record PinLinkSuccess(string PinLink) : PinLinkResult;

    public record PinLinkFailure(string ErrorMessage) : PinLinkResult;
}