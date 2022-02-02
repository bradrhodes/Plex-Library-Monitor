namespace Plibmon.Domain;

public abstract record StorageWriteResult
{
    public record Success() : StorageWriteResult;
    public record Failure(string Reason) : StorageWriteResult;
}