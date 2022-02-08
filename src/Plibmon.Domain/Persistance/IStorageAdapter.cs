namespace Plibmon.Domain.Persistance;

internal interface IStorageAdapter
{
    Task StoreObject(object data, CancellationToken cancellationToken);
    Task StoreObject(object data, CancellationToken cancellationToken, string objectName, int retryCount = 0);
    Task<StorageReadResult<T>> RetrieveObject<T>(CancellationToken cancellationToken);
    Task<StorageReadResult<T>> RetrieveObject<T>(string objectName, CancellationToken cancellationToken);
}