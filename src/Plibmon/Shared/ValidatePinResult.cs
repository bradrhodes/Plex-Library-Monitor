namespace Plibmon.Shared;

public abstract record ValidatePinResult
{
    public record PinValidated() : ValidatePinResult;

    public record PinNotValid() : ValidatePinResult;

    public record PinNotFound() : ValidatePinResult;
}