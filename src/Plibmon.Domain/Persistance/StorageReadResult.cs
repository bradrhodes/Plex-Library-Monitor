namespace Plibmon.Domain.Persistance;

public abstract record StorageReadResult<T>
{
    public record Success(T Data) : StorageReadResult<T>;

    public record Failure() : StorageReadResult<T>;
}