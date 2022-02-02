namespace Plibmon.Domain;

internal interface IStorageAdapter
{
    Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken);
    Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken, string objectName);
    Task<StorageReadResult<T>> RetrieveObject<T>(CancellationToken cancellationToken);
    Task<StorageReadResult<T>> RetrieveObject<T>(string objectName, CancellationToken cancellationToken);
}