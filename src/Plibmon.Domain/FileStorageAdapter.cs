using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Plibmon.Domain;

class FileStorageAdapter : IStorageAdapter
{
    private readonly FileStorageSettings _settings;

    public FileStorageAdapter(FileStorageSettings settings)
    {
        _settings = settings;
    }

    public Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken)
        => StoreObject(data, cancellationToken, string.Empty);

    public async Task<StorageWriteResult> StoreObject(object data,
        CancellationToken cancellationToken, string objectName)
    {
        try
        {
            // Get the data from the current file, mutate it and overwrite it
            var jsonString = await File.ReadAllTextAsync(_settings.FullFileName, cancellationToken)
                .ConfigureAwait(false);

            JObject jObject;

            try
            {
                jObject = JsonConvert.DeserializeObject<JObject>(jsonString);
            }
            catch // This means the file is empty or not a json object at all
            {
                jObject = new JObject();
            }

            jObject[objectName] = JObject.FromObject(data);

            await File.WriteAllTextAsync(_settings.FullFileName, jObject.ToString(), cancellationToken).ConfigureAwait(false);

            return new StorageWriteResult.Success();
        }
        catch (Exception ex)
        {
            return new StorageWriteResult.Failure(ex.Message);
        }
    }

    public Task<StorageReadResult<T>> RetrieveObject<T>(CancellationToken cancellationToken)
        => RetrieveObject<T>(objectName: typeof(T).Name, cancellationToken);

    public async Task<StorageReadResult<T>> RetrieveObject<T>(string objectName,
        CancellationToken cancellationToken)
    {
        if (!File.Exists(_settings.FullFileName))
            return new StorageReadResult<T>.Failure();
            
        var jsonString = await File.ReadAllTextAsync(_settings.FullFileName, cancellationToken).ConfigureAwait(false);;
        var data = JsonConvert.DeserializeObject<JObject>(jsonString);

        if (data == null)
            return new StorageReadResult<T>.Failure();

        var result = data[objectName].ToObject<T>();

        return new StorageReadResult<T>.Success(result);
    }

    private void UpsertDataFolder()
    {
        if (!Directory.Exists(_settings.FolderName))
            Directory.CreateDirectory(_settings.FolderName);
    }
}