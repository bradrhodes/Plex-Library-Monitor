using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Plibmon.Domain.Persistance;

class FileStorageAdapter : IStorageAdapter
{
    private readonly FileStorageSettings _settings;
    private readonly ILogger<FileStorageAdapter> _logger;

    public FileStorageAdapter(FileStorageSettings settings, ILogger<FileStorageAdapter> logger)
    {
        _settings = settings;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StoreObject(object data, CancellationToken cancellationToken)
        => StoreObject(data, cancellationToken, data.GetType().Name);

    public async Task StoreObject(object data,
        CancellationToken cancellationToken, string objectName, int retryCount = 0)
    {
        await EnsureFile().ConfigureAwait(false);
        try
        {
            // Get the data from the current file, mutate it and overwrite it
            var jsonString = await File.ReadAllTextAsync(_settings.FullFileName, cancellationToken)
                .ConfigureAwait(false);

            var jObject = JsonConvert.DeserializeObject<JObject>(jsonString) ?? new JObject();

            jObject[objectName] = JObject.FromObject(data);

            await File.WriteAllTextAsync(_settings.FullFileName, jObject.ToString(), cancellationToken)
                .ConfigureAwait(false);
            _logger.LogInformation($"{objectName} stored successfully");
        }
        // Super naive retry mechanism for concurrent access to the data file
        // This won't scale but it doesn't need to
        catch (IOException ex)
        {
            if (retryCount >= 5)
                throw;
            
            await Task.Delay(500, cancellationToken);

            await StoreObject(data, cancellationToken, objectName, retryCount+1);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unable to store {objectName}. {ex.Message}");
        }
    }

    private async Task EnsureFile()
    {
        if (!Directory.Exists(_settings.FolderName))
        {
            _logger.LogInformation($"Data folder ({_settings.FolderName}) does not exist. Creating it.");
            try
            {
                Directory.CreateDirectory(_settings.FolderName);
                _logger.LogInformation($"Data folder ({_settings.FolderName}) created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Data folder({_settings.FolderName}) could not be created. Error: {ex.Message}");
            }
        }
        
        if (!File.Exists(_settings.FullFileName))
        {
            _logger.LogInformation($"Data file ({_settings.FullFileName}) does not exist. Creating it.");
            try
            {
                await File.WriteAllTextAsync(_settings.FullFileName, string.Empty);
                _logger.LogInformation($"Data file ({_settings.FullFileName}) created.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Data file ({_settings.FullFileName}) could not be created. Error: {ex.Message}");
            }
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